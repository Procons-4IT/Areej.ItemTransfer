
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using Areej.ItemTransfer.Main;

namespace Areej.ItemTransfer.UI
{

    [FormAttribute("720", "Forms/Goods Issue.b1f")]
    public class Goods_Issue : B1SystemForm
    {
        private SAPbouiCOM.Matrix GIMatrix;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditTextToBranch;
        private SAPbouiCOM.EditText EditTextToWhs;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.DBDataSource OIGE;
        private string frmWhs = string.Empty;
        private string toWhs = string.Empty;
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.Button0.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button0_ClickAfter);
            this.GIMatrix = ((SAPbouiCOM.Matrix)(this.GetItem("13").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.EditTextToBranch = ((SAPbouiCOM.EditText)(this.GetItem("Item_2").Specific));
            this.EditTextToWhs = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
            this.EditTextToWhs.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.EditTextWhsUDF_ChooseFromListAfter);         
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.OIGE = this.UIAPIRawForm.DataSources.DBDataSources.Item("OIGE");
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {


        }



        private void OnCustomInitialize()
        {
            //Grab the DataSource
            //EditTextFromWhs.DataBind.SetBound(true, "OIGE", "U_fromWhs");
            EditTextToWhs.ChooseFromListUID = "CFL_15";
            EditTextToWhs.ChooseFromListAlias = "WhsCode";

        }

        private void Button0_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.FormMode == Convert.ToInt32(SAPbouiCOM.BoFormMode.fm_OK_MODE))
            {
                var grForm = Application.SBO_Application.OpenForm(SAPbouiCOM.BoFormObjectEnum.fo_GoodsReceipt, "", "");
                grForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;
                Goods_Receipt grF = B1SystemForm.goodsReceiptForms[grForm.UniqueID];
                List<Item> items = GetItems();
                grF.populateItemsFromGR(items);
            }
        }

        private List<Item> GetItems()
        {
            List<Item> selectedItems = new List<Item>();
            for (int i = 1; i <= GIMatrix.RowCount; i++)
            {
                Item item = new Item
                {
                    ItemCode = GIMatrix.GetCellValue("1", i).ToString()
                    ,Quantity = Convert.ToDouble(GIMatrix.GetCellValue("9", i).ToString())
                    ,BaseDocEntry = 0,BaseDocType = 0
                    ,WhsCode = EditTextToWhs.Value.ToString()
                };

                selectedItems.Add(item);
            }
            return selectedItems;

        }

        private void EditTextWhsUDF_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                var ppVal = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
                string WhsCode = ppVal.SelectedObjects.GetValue(0, 0).ToString();
                this.UIAPIRawForm.Visible = true;
                SAPbouiCOM.EditText tempEditText = EditTextToWhs;
   
                if (pVal.ItemUID == EditTextToWhs.Item.UniqueID)
                {
                    //Add the To Branch
                    var whsRecordSet = B1Helper.DiCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
                    string query = string.Format("SELECT branch.BPLName FROM OWHS whs inner join OBPL branch on whs.BPLid = branch.BPLId Where whs.WhsCode = '{0}'", WhsCode);
                    whsRecordSet.DoQuery(query);
                    try
                    {
                        EditTextToBranch.Active = true;
                        EditTextToBranch.Value = whsRecordSet.Fields.Item(0).Value.ToString();
                    }
                    catch (Exception ex)
                    {
                        Utilities.LogErrors(string.Concat("Exception Occured at GoodsIssue.ChoseFromListAfter: ",ex.ToString()));
                    }
                 
                }
                else
                {
                    //tempEditText = EditTextFromWhs;
                }
                //var tempEditText = pVal.ItemUID == EditTextFromWhs.Item.UniqueID ? EditTextFromWhs : EditTextToWhs;
         

                tempEditText.Active = true;
                tempEditText.String = WhsCode;

            }
            catch (Exception ex)
            {
                Utilities.LogErrors(string.Concat("Exception Occured at GoodsIssue.ChoseFromListAfter: ", ex.ToString()));

            }




        }




    }
}
