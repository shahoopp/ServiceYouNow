
using System;
using System.Collections.Generic;

namespace ServiceYouNow.src.catalog
{
    public static class CatalogItemRegistry
    {
        public static readonly Dictionary<string, CatalogItemConfig> Items =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["PSG_DB_AR"] = new CatalogItemConfig
                {
                    Name = "PSG Database Access Request",
                    ActionTypes = new List<string> { "Add Temporary", "Add Permanent", "Remove Permanent" },
                    Environments = new List<string> { "CNV", "DEV", "EBF", "ETS", "P02", "P03", "PRD", "QA", "SAT", "SPR", "TRN", "UAT" },
                    ServerName = true,
                    AccessTypes = new List<string> { "Admin", "DBOwner", "Read", "ReadWrite" },
                    AssociatedWorkItemRequired = true,
                    BusinessCaseRequired = true,
                    ServerNamesByEnvironment = new Dictionary<string, List<string>>
                    {
                        ["EBF"] = new List<string>
                        {
                            "VEB1PSARISQL01",
                            "VEB1PSARISQL01_GP",
                            "VEB1PSARISQL01_KOFAX",
                            "VEB1PSCONSQL01",
                            "VEB1PSCONSQL01_GP",
                            "VEB1PSCRMSQL02",
                            "VEB1PSPCOSQL01",
                            "VEB1PSSPSQL02"
                        },
                        ["PRD"] = new List<string>
                        {
                            "VEB1PSARISQL01",
                            "VEB1PSARISQL01_GP",
                            "VEB1PSARISQL01_KOFAX",
                            "VEB1PSCONSQL01",
                            "VEB1PSCONSQL01_GP",
                            "VEB1PSCRMSQL02",
                            "VEB1PSPCOSQL01",
                            "VEB1PSSPSQL02"
                        },
                        ["SAT"] = new List<string>
                        {
                            "AZNPIDWAUXDATASQLSRV01",
                            "AZNPIDWAUXDATASQLSRV02",
                            "AZNPMISQL01",
                            "AZNPSQLSRVMAIN01",
                            "AZSATAPEXSQL01",
                            "AZSATAUXSQL01",
                            "AZSATHVRHUB02",
                            "AZSATHVRHUB03",
                            "AZSATPBRS01",
                            "AZSATPSCONSQL01",
                            "AZSATPSJOBSQL01",
                            "AZSATTAP01",
                            "AZSATXPERTSQL01",
                            "VDV1PSCONSQL01_GP",
                            "VDV1PSDEPSQL01",
                            "VDV1PSMIGSQL01",
                            "VDV1PSTISQL02",
                            "VQA1PSMCSQL01",
                            "VQA1PSSODNSQL01",
                            "VUA1HCOMSQLMI01",
                            "VUA1PSMCSQL01",
                            "VUA1PSTISQL01"
                        }
                    }
                },
                ["MS_SQL_DB_ISG"] = new CatalogItemConfig
                {
                    Name = "Microsoft SQL Database Access (ISG) (In Progress)"
                    
                }
            };
    }
}
