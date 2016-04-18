﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Areej.ItemTransfer.Main
{
   public class AddonInfoInfo
    {
       public static bool InstallUDFs()
       {
           try
           {
               //Add The From Branch
               B1Helper.AddField("toWhs", "To Warehouse", "OIGE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoYesNoEnum.tNO, false);
               B1Helper.AddField("fromWhs", "From Warehouse", "OIGE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoYesNoEnum.tNO, false);
               B1Helper.AddField("toBranch", "To Branch", "OIGE", SAPbobsCOM.BoFieldTypes.db_Alpha, 50,SAPbobsCOM.BoYesNoEnum.tNO, false);
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
            
       }

    }
}
