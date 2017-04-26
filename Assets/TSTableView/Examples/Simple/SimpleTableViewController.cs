using UnityEngine;
using System.Collections;
using Tacticsoft;
using System.Runtime.InteropServices;
using Mono.Data.SqliteClient;
using System.Data;
using System;

namespace Tacticsoft.Examples
{
    //An example implementation of a class that communicates with a TableView
    public class SimpleTableViewController : MonoBehaviour, ITableViewDataSource
    {
        public VisibleCounterCell m_cellPrefab;
        public TableView m_tableView;
		public GameObject tableViewObject;
		public GameObject scrollingIndicatorObject;

        public int m_numRows;
        private int m_numInstancesCreated = 0;
		private string[] data_arr;
		private int arr_num = 0;

        //Register as the TableView's delegate (required) and data source (optional)
        //to receive the calls
        void Start() {
            m_tableView.dataSource = this;
        }

        //public void SendBeer() {
        //   Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?business=contact@tacticsoft.net&cmd=_xclick&item_name=Beer%20for%20TSTableView&currency_code=USD&amount=5.00");
        //}

        #region ITableViewDataSource

        //Will be called by the TableView to know how many rows are in this table
        public int GetNumberOfRowsForTableView(TableView tableView) {

			string con = "URI=file:" + Application.persistentDataPath + "/ARibooks.db";
			IDbConnection dbconn = (IDbConnection)new SqliteConnection (con);
			dbconn.Open ();
			IDbCommand dbcmd = dbconn.CreateCommand ();
			string sqlCount = "SELECT count(*) as thecount FROM books";
			dbcmd.CommandText = sqlCount;
			object obj = dbcmd.ExecuteScalar ();
			int count = (int)Convert.ToInt32 (obj);
			if (count == 0) {
				count = 1;
				tableViewObject.SetActive (false);
				scrollingIndicatorObject.SetActive (false);
			} else {
				tableViewObject.SetActive (true);
				scrollingIndicatorObject.SetActive (true);
			}

            return count;
        }

        //Will be called by the TableView to know what is the height of each row
        public float GetHeightForRowInTableView(TableView tableView, int row) {
            return (m_cellPrefab.transform as RectTransform).rect.height;
        }

        //Will be called by the TableView when a cell needs to be created for display
        public TableViewCell GetCellForRowInTableView(TableView tableView, int row) {
            VisibleCounterCell cell = tableView.GetReusableCell(m_cellPrefab.reuseIdentifier) as VisibleCounterCell;
            if (cell == null) {
                cell = (VisibleCounterCell)GameObject.Instantiate(m_cellPrefab);
                cell.name = "VisibleCounterCellInstance_" + (++m_numInstancesCreated).ToString();
            }

			string con = "URI=file:" + Application.persistentDataPath + "/ARibooks.db";
			IDbConnection dbconn = (IDbConnection)new SqliteConnection (con);
			dbconn.Open ();
			IDbCommand dbcmd = dbconn.CreateCommand ();
			row += 1;//row level start 0
			string name = "", path = "";//to access data
			string roll_name = "", roll_path = "";//to save row data by comparing row level


			string sqlRowQuery = "SELECT * FROM books ORDER BY id DESC";
			dbcmd.CommandText = sqlRowQuery;
			IDataReader reader = dbcmd.ExecuteReader ();

			int row_count = 1;
			while (reader.Read ()) {
				name = reader.GetString (1);
				path = reader.GetString (2);
				if (row_count == row) {
					roll_name = name;
					roll_path = path;
				}

				Debug.Log ("count : " + row_count++);
			}

			reader.Close ();
			reader = null;

			cell.SetRowNumber(roll_name, roll_path);
            return cell;
        }

        #endregion

        #region Table View event handlers

        //Will be called by the TableView when a cell's visibility changed
        public void TableViewCellVisibilityChanged(int row, bool isVisible) {
            //Debug.Log(string.Format("Row {0} visibility changed to {1}", row, isVisible));
            if (isVisible) {
                VisibleCounterCell cell = (VisibleCounterCell)m_tableView.GetCellAtRow(row);
                cell.NotifyBecameVisible();
            }
        }

        #endregion

    }
}
