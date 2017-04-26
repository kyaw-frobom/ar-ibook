/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.InteropServices;
using Mono.Data.SqliteClient;
using System.Data;
using System;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler : MonoBehaviour,
                                                ITrackableEventHandler
	{
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
		private Texture2D NewBookCoverTexture;
		private IEnumerator coroutine;
		private IEnumerator isbn_coroutine;
		private string BookName, BookPath;

    
        #endregion // PRIVATE_MEMBER_VARIABLES

		#if UNITY_IPHONE
		[DllImport ("__Internal")]

		private static extern void lunchApp(string s);

		#endif

		#region PUBLIC_MEMBER_VARIABLES

		public Text menuPanelTitle;
		public Text userManualText;
		public GameObject bookCover;
		public Button btnAdd;
		public Button btnCancel;

		public Text BookPathText;

		#endregion // PUBLIC_MEMBER_VARIABLES
	

        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }

			//db connection
			string conn = "URI=file:" + Application.persistentDataPath + "/ARibooks.db";
			IDbConnection dbconn;
			dbconn = (IDbConnection)new SqliteConnection (conn);
			dbconn.Open ();
			Debug.Log ("db connection opened");
			Debug.Log ("db path" + conn);
			IDbCommand dbcmd = dbconn.CreateCommand ();
			//to clean database
			//string sqlQuery4 = "DROP TABLE IF EXISTS books";
			//dbcmd.CommandText = sqlQuery4;
			//dbcmd.ExecuteNonQuery ();

			string sqlQuery = "CREATE TABLE IF NOT EXISTS books (id INTEGER NOT NULL PRIMARY KEY, name VARCHAR(40), path VARCHAR(40))";

			dbcmd.CommandText = sqlQuery;
			dbcmd.ExecuteNonQuery ();
			dbcmd.Dispose ();
			dbcmd = null;
			dbconn.Close ();
			dbconn = null;
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS



        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }

        #endregion // PUBLIC_METHODS



        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			BookName = mTrackableBehaviour.TrackableName;//set book name for db store


			//getting Book Cover URL with the Name of target we found.
			string coverURL = "http://www.frobom.com/cinema/covers/" + mTrackableBehaviour.TrackableName + ".jpg";
			Debug.Log ("Cover URL " + coverURL);//debugging cover url
			//set the texture2D setting
			NewBookCoverTexture = new Texture2D (4, 4, TextureFormat.ARGB32, false);
			//loading the Image URL in web and set to a texture 
			coroutine = LoadCover (coverURL);
			StartCoroutine (coroutine);

			//getting isbn code URL with the Name of target we found.
			string isbn_code = "http://www.frobom.com/cinema/covers/" + mTrackableBehaviour.TrackableName + ".txt";
			isbn_coroutine = LoadISBN (isbn_code);
			StartCoroutine (isbn_coroutine);
	
	
        }

        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

			menuPanelTitle.text = " ";//set the Book Title in Navi bar when lost
        }

        #endregion // PRIVATE_METHODS

		#region PUBLIC_METHODS
		//to load book cover from server when the target is detected
		public IEnumerator LoadCover(string urls)
		{
			WWW www = new WWW(urls);
			yield return www;//waiting URL loading is finished
		
			www.LoadImageIntoTexture (NewBookCoverTexture);// set the loaded image to a texture
			Debug.Log ("Cover loaded by URL is worked!!");
			menuPanelTitle.text = BookName;//set the Book Title in Navi bar when found

			userManualText.enabled = false;//hidden usage message on screen
			bookCover.SetActive (true);//showing the Book Cover frame 
			//set texture to Book Cover Frame
			bookCover.GetComponent<RawImage>().texture = (Texture) NewBookCoverTexture;

			btnAdd.gameObject.SetActive (true);//showing Add button
			btnCancel.gameObject.SetActive (true);//showing Cancel button
		}


		public IEnumerator LoadISBN(string urls){
			WWW www = new WWW (urls);
			yield return www;

			if (www.error != null) {
				Debug.Log ("Error.." + www.error);
			} else {
				Debug.Log ("Found.." + www.text);
				BookPath = www.text;
				Debug.Log ("Book Path : " + BookPath);
				BookPathText.text = BookPath;
				//add to db
				//db connection
				string conn = "URI=file:" + Application.persistentDataPath + "/ARibooks.db";
				IDbConnection dbconn;
				dbconn = (IDbConnection)new SqliteConnection (conn);
				dbconn.Open ();
				IDbCommand dbcmd = dbconn.CreateCommand ();

				string sqlQuery2 = "INSERT INTO books (name, path) VALUES ( '"+ BookName +"', '"+ BookPath +"')";
				dbcmd.CommandText = sqlQuery2;
				dbcmd.ExecuteNonQuery ();
				Debug.Log ("Data Inserted!!");
			}
		}

		public void btnAddClicked()
		{
			//this is to direct the built-in ibook app
			//it's calling Assets/Plugins/iOS/AppLunchPlugin.mm
			#if UNITY_IPHONE
				lunchApp(BookPathText.text);

			#endif
		}

		#endregion


    }
}
