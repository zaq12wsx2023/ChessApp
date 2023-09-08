using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CHESS
{
    public class ChessAPI
    {
        public class API_Config
        {

            public string URI_1 { get; set; }
            public string URI_2 { get; set; }

            public string OperationId { get; set; }

        }

        public class API_1_Response
        {
            public string Response { get; set; }
        }

        public class API_2_Response
        {
            public string starting { get; set; }
            public string ending { get; set; }
            public string shortestPath { get; set; }
            public string numberOfMoves { get; set; }
            public string isFound { get; set; }
            public string operationId { get; set; }
            public string id { get; set; }
            public string createdOn { get; set; }
            public string lastUpdatedOn { get; set; }
            public string minimumRetentionDate { get; set; }
            public string isDeleted { get; set; }
            public string deletedOn { get; set; }
            public string cosmosDbResourceId { get; set; }
            public string comsosDbSelfReference { get; set; }
            public string cosmosDbEtag { get; set; }
            public string cosmosDbAttachmentsReference { get; set; }
            public string cosmosDbTimestamp { get; set; }
        }



    }

}
