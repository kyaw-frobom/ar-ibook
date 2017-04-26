using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Mono.Data.SqliteClient;
using System.Data;
using System;


namespace Tacticsoft.Examples
{
    //Inherit from TableViewCell instead of MonoBehavior to use the GameObject
    //containing this component as a cell in a TableView
    public class VisibleCounterCell : TableViewCell
    {
		#if UNITY_IPHONE
		[DllImport ("__Internal")]

		private static extern void lunchApp(string s);

		#endif

        public Text m_rowNumberText;
        public Button m_rowButton;
		string bookPath = "";

		public void SetRowNumber(string name , string path) {
			m_rowNumberText.text = name;
			bookPath = path;
			m_rowButton.onClick.AddListener (OpenIBook);

        }

		void OpenIBook(){
			Debug.Log ("You have opened ibook : " + bookPath);
			#if UNITY_IPHONE
				Debug.Log ("Book Path ios : " + bookPath);
				lunchApp(bookPath);
			#endif
		}

        private int m_numTimesBecameVisible;
        public void NotifyBecameVisible() {
            m_numTimesBecameVisible++;
            //m_visibleCountText.text = "# rows this cell showed : " + m_numTimesBecameVisible.ToString();
        }

    }
}
