using SQLite;

namespace LiteSql.Util
{
    public class DBUtil
    {
        public static List<List<string>> getResultList(SQLiteConnection con, string query) {
            List<List<string>> ResultList = new List<List<string>>();
            SQLitePCL.sqlite3_stmt stQuery = SQLite3.Prepare2(con.Handle, query);
            int colLenght = SQLite3.ColumnCount(stQuery);

            var columnNamesList = new List<string>();
            for (int i = 0; i < colLenght; i++)
            {
                columnNamesList.Add(SQLite3.ColumnName(stQuery, i));
            }
            //CommonUtil.Move(columnNamesList, "row_id", colLenght);
            ResultList.Add(columnNamesList);


            while (SQLite3.Step(stQuery) == SQLite3.Result.Row)
            {
                var rowDataList = new List<string>();
                for (int i = 0; i < colLenght; i++)
                {
                    var columnType = SQLitePCL.raw.sqlite3_column_decltype(stQuery, i).utf8_to_string();

                    switch (columnType)
                    {
                        case "TEXT":
                            rowDataList.Add(CommonUtil.NVL(SQLite3.ColumnString(stQuery, i), "NULL"));
                            break;
                        case "INTEGER":
                            rowDataList.Add(CommonUtil.NVL(SQLite3.ColumnInt(stQuery, i).ToString(),"NULL"));
                            break;
                        case "DOUBLE":
                            rowDataList.Add(CommonUtil.NVL(SQLite3.ColumnDouble(stQuery, i).ToString(), "NULL"));
                            break;
                        default:
                            rowDataList.Add("NULL");
                            break;
                    }
                }
                ResultList.Add(rowDataList);
            }
  
            return ResultList;
        }
    }
}
