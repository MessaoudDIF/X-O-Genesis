﻿using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetvetPOS_Inventory_System
{
    public class ProductMapper: DatabaseMapper
    {
        public ProductMapper(MySqlDB db)
            : base(db)
        {
            tableName = "product_tbl";
            id = "id";
            fieldsname = new string[] {
                "id",
                "name",
                "unit_price",
                "specification",
                "warranty",
                "replacement",
                "category_id",
            };
        }

        public bool deactiveProduct(string barcode){
            string condition = String.Format(" id = '{0}' ", barcode);
            updateSet(condition, "active = 0");
            return update(updateQuery);
        }

        public bool updateDescription(string barcode, string newDescription)
        {
            string condition = String.Format("id = '{0}' ", barcode);
            updateSet(condition, String.Format("name = '{0}'", newDescription));
            return update(updateQuery);
        }

        public bool updateUnitPrice(string barcode, decimal newPrice)
        {
            string condition = String.Format("id = '{0}'", barcode);
            updateSet(condition, String.Format("name = {0}", newPrice));
            return update(updateQuery);
        }

        public bool updateCategory(string barcode, string newCategory)
        {
            string condition = String.Format("id = '{0}'", barcode);
            updateSet(condition, String.Format("category_id = {0}", newCategory));
            return update(updateQuery);
        }

        public bool updateSourceCompanyName(string barcode, string company)
        {
            string condition = String.Format("id = '{0}'", barcode);
            updateSet(condition, String.Format("source_company_name = '{0}'", company));
            return update(updateQuery);
        }

        public string createProduct(Product product)
        {
            return insertValues(
                product.Barcode,
                product.Description, 
                product.UnitPrice,
                product.Specification,
                product.Warranty,
                product.Replacement,
                product.Category_id
                );
        }

        public Product getProductFromName(string name)
        {
            return new Product(getEntityWhere(String.Format("name = '{0}'", name)));
        }

        public Product getProductFromBarcode(string barcode)
        {
            return new Product(getEntityFromId(barcode));
        }

        public bool updateProduct(Product origProduct, Product updatedProduct)
        {
            if (origProduct.Barcode != updatedProduct.Barcode || origProduct.Equals(updatedProduct))
                return false;

            string desc = string.Empty, unitPrice = string.Empty, category = string.Empty, manufacturer = string.Empty, specs = string.Empty,
                warranty = string.Empty, replacement = string.Empty;

            if (origProduct.Description != updatedProduct.Description)
                desc = string.Format("name = '{0}'", updatedProduct.Description);
            if (origProduct.UnitPrice != updatedProduct.UnitPrice)
                unitPrice = string.Format("unit_price = {0}", updatedProduct.UnitPrice);
            if (origProduct.Company != updatedProduct.Company)
                manufacturer = string.Format("source_company_name = '{0}'", updatedProduct.Company);
            if (origProduct.Category_id != updatedProduct.Category_id)
                category = string.Format("category_id = {0}", updatedProduct.Category_id);
            if (origProduct.Specification != updatedProduct.Specification)
                specs = string.Format("specification = '{0}'", updatedProduct.Specification);
            if (origProduct.Warranty != updatedProduct.Warranty)
                warranty = string.Format("warranty = '{0}'", updatedProduct.Warranty);
            if (origProduct.Replacement != updatedProduct.Replacement)
                replacement = string.Format("replacement = '{0}'", updatedProduct.Replacement);

            string condition = string.Format("id = '{0}'", origProduct.Barcode);

            return update(
                updateSet(condition, desc, unitPrice, category, manufacturer, specs, warranty, replacement)
                );
        }
    }


}
