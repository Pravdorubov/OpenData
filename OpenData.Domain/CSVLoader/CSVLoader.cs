using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;

namespace OpenData.Domain.CSVLoader
{
    public class CSVLoader
    {
        private static string fileCSV;		//full file name
        private static string dirCSV;		//directory of file to import
        private static string fileNevCSV;	//name (with extension) of file to import - field

        public static string FileNevCSV	//name (with extension) of file to import - property
        {
            get { return fileNevCSV; }
            set { fileNevCSV = value; }
        }

        private static string strFormat="Delimited(;)";	//CSV separator character
        private static string strEncoding = "ANSI"; //Encoding of CSV file

        private static long rowCount = 0;	//row number of source file



        // Browses file with OpenFileDialog control

        
        // Delimiter character selection
        private void Format()
        {
            //try
            //{

            //    if (rdbSemicolon.Checked)
            //    {
                    strFormat = "Delimited(;)";
            //    }
            //    else if (rdbTab.Checked)
            //    {
            //        strFormat = "TabDelimited";
            //    }
            //    else if (rdbSeparatorOther.Checked)
            //    {
            //        strFormat = "Delimited(" + txtSeparatorOtherChar.Text.Trim() + ")";
            //    }
            //    else
            //    {
            //        strFormat = "Delimited(;)";
            //    }


            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Format");
            //}
            //finally
            //{
            //}
        }


        // Encoding selection
        private void Encoding()
        {
            //try
            //{

            //    if (rdbAnsi.Checked)
            //    {
                    strEncoding = "Unicode";
            //    }
            //    else if (rdbUnicode.Checked)
            //    {
            //        strEncoding = "Unicode";
            //    }
            //    else if (rdbOEM.Checked)
            //    {
            //        strEncoding = "OEM";
            //    }
            //    else
            //    {
            //        strEncoding = "ANSI";
            //    }


            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Encoding");
            //}
            //finally
            //{
            //}
        }



        /* Schema.ini File (Text File Driver)

        When the Text driver is used, the format of the text file is determined by using a
        schema information file. The schema information file, which is always named Schema.ini
        and always kept in the same directory as the text data source, provides the IISAM 
        with information about the general format of the file, the column name and data type
        information, and a number of other data characteristics*/

        private static void writeSchema()
        {
            try
            {
                FileStream fsOutput = new FileStream(dirCSV + "\\schema.ini", FileMode.Create, FileAccess.Write);
                StreamWriter srOutput = new StreamWriter(fsOutput);
                string s1, s2, s3, s4, s5;

                s1 = "[" + FileNevCSV + "]";
                s2 = "ColNameHeader=true";// +chkFirstRowColumnNames.Checked.ToString();
                s3 = "Format=" + strFormat;
                s4 = "MaxScanRows=25";
                s5 = "CharacterSet=" + strEncoding;

                srOutput.WriteLine(s1.ToString() + "\r\n" + s2.ToString() + "\r\n" + s3.ToString() + "\r\n" + s4.ToString() + "\r\n" + s5.ToString());
                srOutput.Close();
                fsOutput.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "writeSchema");
            }
            finally
            { }
        }

        /*
         * Loads the csv file into a DataSet.
         * 
         * If the numberOfRows parameter is -1, it loads oll rows, otherwise it
         * loads the first specified number of rows (for preview)
         */

        public static DataSet LoadCSV(int numberOfRows)
        {
            DataSet ds = new DataSet();
            try
            {
                // Creates and opens an ODBC connection
                string strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + dirCSV.Trim() + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                string sql_select;
                OdbcConnection conn;
                conn = new OdbcConnection(strConnString.Trim());
                conn.Open();

                //Creates the select command text
                if (numberOfRows == -1)
                {
                    sql_select = "select * from [" + FileNevCSV.Trim() + "]";
                    
                }
                else
                {
                    sql_select = "select top " + numberOfRows + " * from [" + FileNevCSV.Trim() + "]";
                }

                //Creates the data adapter
                OdbcDataAdapter obj_oledb_da = new OdbcDataAdapter(sql_select, conn);

                //Fills dataset with the records from CSV file
                obj_oledb_da.Fill(ds, "csv");

                //closes the connection
                conn.Close();
            }
            catch (Exception e) //Error
            {
                //MessageBox.Show(e.Message, "Error - LoadCSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ds;
        }


        // Checks if a file was given.
        private static bool fileCheck(string fileCSV, string dirCSV)
        {
            if ((fileCSV == "") || (fileCSV == null) || (dirCSV == "") || (dirCSV == null) || (FileNevCSV == "") || (FileNevCSV == null))
            {
                //MessageBox.Show("Select a CSV file to load first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }
        private void loadPreview()
        {
            try
            {
                // select format, encoding, an write the schema file
                //Format();
                //Encoding();
                //writeSchema();

                // loads the first 500 rows from CSV file, and fills the
                // DataGridView control.
                //this.dataGridView_preView.DataSource = LoadCSV(500);
                //this.dataGridView_preView.DataMember = "csv";
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error - loadPreview", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     


        /*
         * Loads the CSV file to a dataset, and imports data
         * to the database with SqlBulkCopy.
         */

        public static  void SaveToDatabase_withDataSet(string filePath,string conStr, string tableName)
        {
            fileCSV = filePath;
            string[] stringSeparators = new string[] { @"\" };
            string[] astr = filePath.Split(stringSeparators, StringSplitOptions.None);
            dirCSV = fileCSV.Replace(astr[astr.Length - 1], "");
            fileNevCSV=astr[astr.Length - 1];

            try
            {
                if (fileCheck(fileCSV,dirCSV))
                {
                    // select format, encoding, and write the schema file
                    //Format();
                    //Encoding();
                    writeSchema();

                    // loads all rows from from csv file
                    DataSet ds = LoadCSV(-1);

                    // gets the number of rows
                    rowCount = ds.Tables[0].Rows.Count;

                    // Makes a DataTableReader, which reads data from data set.
                    // It is nececery for bulk copy operation.
                    DataTableReader dtr = ds.Tables[0].CreateDataReader();

                    // Creates schema table. It gives column names for create table command.
                    DataTable dt;
                    dt = dtr.GetSchemaTable();

                    // You can view that schema table if you want:
                    //this.dataGridView_preView.DataSource = dt;

                    // Creates a new and empty table in the sql database
                    CreateTableInDatabase(dt, "dbo", tableName, conStr);

                    // Copies all rows to the database from the dataset.
                    using (SqlBulkCopy bc = new SqlBulkCopy(conStr))
                    {
                        // Destination table with owner - this example doesn't
                        // check the owner and table names!
                        bc.DestinationTableName = "[dbo].[" + tableName + "]";

                        // User notification with the SqlRowsCopied event
                        //bc.NotifyAfter = 100;
                        //bc.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

                        // Starts the bulk copy.
                        bc.WriteToServer(ds.Tables[0]);

                        // Closes the SqlBulkCopy instance
                        bc.Close();
                    }

                    // Writes the number of imported rows to the form
                    //this.lblProgress.Text = "Imported: " + this.rowCount.ToString() + "/" + this.rowCount.ToString() + " row(s)";
                    //this.lblProgress.Refresh();

                    // Notifies user
                    //MessageBox.Show("ready");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error - SaveToDatabase_withDataSet", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CSVToDataBase(string filePath, string conStr, string tableName)
        {
            // Создаем ридер для источника, с которым ходим работать.
            //var reader = GetReader();

            // Строка подключения.
            var connectionString = @"Server={сервер};initial catalog={база данных};Integrated Security=true";

            // Создаем объект загрузчика SqlBulkCopy, указываем таблицу назначения и загружаем.
            using (var loader = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.Default))
            {
                loader.ColumnMappings.Add(0, 2);
                loader.ColumnMappings.Add(1, 1);
                loader.ColumnMappings.Add(2, 3);
                loader.ColumnMappings.Add(3, 4);

                loader.DestinationTableName = "Customers";
                //loader.WriteToServer(reader);

                Console.WriteLine("Загрузили!");
            }

            Console.ReadLine();
        }

        //static IDataReader GetReader()
        //{
        //    var sourceFilepath = AppDomain.CurrentDomain.BaseDirectory + "sqlbulktest.csv";
        //    var convertTable = GetConvertTable();
        //    var constraintsTable = GetConstraintsTable();

        //    var reader = new CSVReader(sourceFilepath, constraintsTable, convertTable);
        //    return reader;
        //}

        static Func<string, bool>[] GetConstraintsTable()
        {
            var constraintsTable = new Func<string, bool>[4];

            constraintsTable[0] = x => !string.IsNullOrEmpty(x);
            constraintsTable[1] = constraintsTable[0];
            constraintsTable[2] = x => true;
            constraintsTable[3] = x => true;
            return constraintsTable;
        }

        static Func<string, object>[] GetConvertTable()
        {
            var convertTable = new Func<object, object>[4];

            // Функция преобразования первого столбца csv файла (фамилия)
            convertTable[0] = x => x;

            // Второго (имя)
            convertTable[1] = x => x;

            // Третьего (дата)
            // Разбираем строковое представление даты по определенному формату.
            convertTable[2] = x =>
            {
                DateTime datetime;
                if (DateTime.TryParseExact(x.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture,
                      DateTimeStyles.None, out datetime))
                {
                    return datetime;
                }
                return null;
            };

            // Четвертого (промо код)
            convertTable[3] = x => Convert.ToInt32(x);

            return convertTable;
        }

        public static void SaveToDatabaseDirectly(string filePath, string conStr, string tableName)
        {
            fileCSV = filePath;
            string[] stringSeparators = new string[] { "\\" };
            string[] astr = filePath.Split(stringSeparators, StringSplitOptions.None);
            dirCSV = fileCSV.Replace(astr[astr.Length - 1], "");
            fileNevCSV = astr[astr.Length - 1];
            
            try
            {
                if (fileCheck(fileCSV,dirCSV))
                {
                    // select format, encoding, and write the schema file
                    //Format();
                    //Encoding();
                    writeSchema();

                    // Creates and opens an ODBC connection
                    string strConnString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + dirCSV.Trim() + ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                    string sql_select;
                    OdbcConnection conn;
                    conn = new OdbcConnection(strConnString.Trim());
                    conn.Open();

                    //Counts the row number in csv file - with an sql query
                    OdbcCommand commandRowCount = new OdbcCommand("SELECT COUNT(*) FROM [" + FileNevCSV.Trim() + "]", conn);
                    rowCount = System.Convert.ToInt32(commandRowCount.ExecuteScalar());

                    // Creates the ODBC command
                    sql_select = "select * from [" + FileNevCSV.Trim() + "]";
                    OdbcCommand commandSourceData = new OdbcCommand(sql_select, conn);

                    // Makes on OdbcDataReader for reading data from CSV
                    OdbcDataReader dataReader = commandSourceData.ExecuteReader();

                    // Creates schema table. It gives column names for create table command.
                    DataTable dt;
                    dt = dataReader.GetSchemaTable();

                    // You can view that schema table if you want:
                    //this.dataGridView_preView.DataSource = dt;

                    // Creates a new and empty table in the sql database
                    CreateTableInDatabase(dt, "dbo", tableName, conStr);

                    // Copies all rows to the database from the data reader.
                    using (SqlBulkCopy bc = new SqlBulkCopy("server=(local);database=Test_CSV_impex;Trusted_Connection=True"))
                    {
                        // Destination table with owner - this example doesn't
                        // check the owner and table names!
                        bc.DestinationTableName = "[dbo].[" + tableName + "]";

                        // User notification with the SqlRowsCopied event
                        //bc.NotifyAfter = 100;
                        //bc.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);

                        // Starts the bulk copy.
                        bc.WriteToServer(dataReader);

                        // Closes the SqlBulkCopy instance
                        bc.Close();
                    }

                    // Writes the number of imported rows to the form
                    //this.lblProgress.Text = "Imported: " + this.rowCount.ToString() + "/" + this.rowCount.ToString() + " row(s)";
                    //this.lblProgress.Refresh();

                    // Notifies user
                    //MessageBox.Show("ready");
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error - SaveToDatabaseDirectly", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /*
         * Generates the create table command using the schema table, and
         * runs it in the sql database.
         */

        private static bool CreateTableInDatabase(DataTable dtSchemaTable, string tableOwner, string tableName, string connectionString)
        {
            try
            {

                // Generates the create table command.
                // The first column of schema table contains the column names.
                // The data type is nvarcher(4000) in all columns.

                string ctStr = "CREATE TABLE [" + tableOwner + "].[" + tableName + "](\r\n";
                for (int i = 0; i < dtSchemaTable.Rows.Count; i++)
                {
                    ctStr += "  [" + dtSchemaTable.Rows[i][0].ToString() + "] [nvarchar](4000) NULL";
                    if (i < dtSchemaTable.Rows.Count)
                    {
                        ctStr += ",";
                    }
                    ctStr += "\r\n";
                }
                ctStr += ")";

                // You can check the sql statement if you want:
                //MessageBox.Show(ctStr);


                // Runs the sql command to make the destination table.

                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand command = conn.CreateCommand();
                command.CommandText = ctStr;
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                return true;

            }
            catch (Exception e)
            {
                
                //MsgBx.Show(e.Message, "CreateTableInDatabase", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
