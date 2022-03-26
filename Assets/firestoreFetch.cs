using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class firestoreFetch : MonoBehaviour
{
    [SerializeField] Button button;
    FirebaseFirestore database;
    ListenerRegistration listener;
    ListenerRegistration onStartListener;
    int sessionStart;
    // Start is called before the first frame update
    void Start()
    {
        sessionStart=0;
        database = FirebaseFirestore.DefaultInstance;
        
       // button.onClick.AddListener(StartUserSession);
        Debug.Log("hello");
         
    }
    
    public void StartUserSession(){
        //when session is started add data to firestore.
        // the app should listen for this data then start the activity of pushing emotion datapoints to firestore
        // usersession -> user1001 -> session: int
        // if bool is true start the app 
        Debug.Log("Button has been pressed");
        onStartListener=
        database.Collection("userSession").Document("user1001")
        .Listen(snapshot => {
            Dictionary<string, object> sessionActivity = snapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in sessionActivity) {
                sessionStart=int.Parse(pair.Value.ToString());
                if (sessionStart==1){
                    //session has started
                    // begin stream of data
                    GetData();
                    Debug.Log("user session has started");
                }
                else{
                    Debug.Log("dmdsjdfhsj");
                }
                Debug.Log(System.String.Format("{0}: {1}", pair.Key, pair.Value));
                // the pair
            }
        });
    }
    void StopUserSession(){
        onStartListener.Stop();
        listener.Stop();
    }
    void GetData(){
        // listen to firebase data
        listener = 
         database.Collection("userInformation").Document("user1001")
        .Collection("Enjoyment")
        .Listen(snapshot => {
            Debug.Log("Callback received query snapshot.");
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents) {
                 Debug.Log(System.String.Format("Document data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> emotion = documentSnapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in emotion)
                {
                    // time, float value
                Debug.Log(System.String.Format("{0}: {1}", pair.Key, pair.Value));
                }

                // Newline to separate entries
                Debug.Log("");
            }
            });
    }
    void OnDestroy(){
        try{
             onStartListener.Stop();
        listener.Stop();
        }
       catch{
           Debug.Log("session cannot be stopped because it hasn't started");
       }
    }
    void StoreImages(){

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
