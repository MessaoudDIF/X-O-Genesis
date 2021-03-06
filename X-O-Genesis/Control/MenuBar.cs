﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PetvetPOS_Inventory_System;

namespace PetvetPOS_Inventory_System
{
    public partial class MenuBar : MyUserControl
    {
        protected List<MenuControl> menuControls;
        protected int currentMenuIndex;
        
        public void initControlLocation()
        {
            int startingPosition = 0;

            foreach (MenuControl menuControl in menuControls){
                menuControl.Location = new System.Drawing.Point(0, startingPosition);
                this.Controls.Add(menuControl);
                startingPosition += 140;
            }
        }

        private void initColor()
        {
            this.BackColor = Properties.Settings.Default.sidebarColor;
        }

        private void hideColor()
        {
            this.BackColor = Color.White;
        }

        public void selectControlbyIndex(int index)
        {
            int n = menuControls.Count;
            if (index >= n)
                return;

            menuControls[index].activateControl();
            currentMenuIndex = index;
        }

        private void navigateByKeys(KeyEventArgs e)
        {
           // TabStop = true;
            int n = menuControls.Count;
            if (!timer1.Enabled)
            {
                if (e.Alt && e.KeyCode == Keys.Oemcomma){
                    currentMenuIndex = (currentMenuIndex == 0) ? (n - 1) : currentMenuIndex - 1;
                    selectControlbyIndex(currentMenuIndex);
                }
                else if (e.Alt && e.KeyCode == Keys.OemPeriod){
                    currentMenuIndex = (currentMenuIndex == (n - 1)) ? 0 : currentMenuIndex + 1;
                    selectControlbyIndex(currentMenuIndex);
                }
                timer1.Start();
            }

        }

        public KeyFunction getMenuBarKeyFunction
        {
            get { return new KeyFunction(navigateByKeys); }
        }

        public void selectControlbyName(Menu name)
        {
            foreach (MenuControl menuControl in menuControls){
                if (menuControl.accessMenuName == name){
                    menuControl.activateControl();
                    currentMenuIndex = menuControls.IndexOf(menuControl);
                    break;
                }
            }
        }

        public void removeMenuControls()
        {
            this.Controls.Clear();
        }

        public MenuBar()
        {
            InitializeComponent();
        }

        public MenuBar(Panel panel, MasterController masterController)
            : base(masterController, panel, true)
        {
            InitializeComponent();
            this.masterController.setMenuBar = this;
            masterController.EmployeeLogin += masterController_EmployeeLogin;
            masterController.EmployeeLogout += masterController_EmployeeLogout;
            masterController.SettingsChanged += masterController_SettingsChanged;
        }

        void masterController_SettingsChanged(object sender, SettingArgs e)
        {
            if (e.settings == Settings.PERSONALIZATION)
            {
                initColor();
            }
        }

        void getFrmMain_Resize(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void masterController_EmployeeLogout(object sender, EventArgs e)
        {
            removeMenuControls();
            hideColor();
        }

        void masterController_EmployeeLogin(object sender, EmployeeArgs e)
        {
            MenuList list;
            Menu menu;
            bool SimpleMode = true;
            dbController = masterController.DataBaseController;
            SimpleMode = dbController.systemSettingsMapper.isInSimpleMode();
            
            switch (e.employee.Position)
            {
                case UserLevel.ADMIN:
                    list = new AdminMenuList(masterController, this);
                    menu = Menu.Home;
                    break;
                case UserLevel.CASHIER:
                    if (SimpleMode)
                        list = new SimpleCashierMenuList(masterController, this);
                    else
                        list = new AdvancedCashierMenuList(masterController, this);
                        
                    menu = Menu.POS;
                    break;
                case UserLevel.INVENTORY_PERSONNEL:
                    list = new InventoryPersonnelMenuList(masterController, this);
                    menu = Menu.Stock_Control;
                    break;
                case UserLevel.STAFF:
                    list = new StaffMenuList(masterController, this);
                    menu = Menu.Orders;
                    break;
                default:
                    list = new AdminMenuList(masterController, this);
                    menu = Menu.Home;
                    break;
            }

            accessMenuControl = list.getList;
            masterController.appearSideAndSubBar();
            initControlLocation();
            selectControlbyName(menu);
            initColor();
        }

        public void updateMenus()
        {
            foreach (MenuControl menuControl in menuControls)
                menuControl.redraw();
        }

        public void unselectAll()
        {
            foreach (MenuControl menuControl in menuControls)
                menuControl.unselect();
        }

        public List<MenuControl> accessMenuControl
        {
            get { return menuControls; }
            set { this.menuControls = value; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void MenuBar_Load(object sender, EventArgs e)
        {

        }
    }
}
