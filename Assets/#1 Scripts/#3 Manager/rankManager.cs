using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class rankManager : MonoBehaviour
{
    class Rank
    {
        public string name;
        public string score;

        public Rank(string name, string score)
        {
            this.name = name;
            this.score = score;
        }
    }

    public DatabaseReference reference { get; set; }

    public void SetRank(int game, string id, string score, bool high_score_good)
    {
        Debug.Log("hi");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                    if (task.Result == DependencyStatus.Available)
                    {
                        var db = FirebaseDatabase.GetInstance("https://returngame-d8a65-default-rtdb.firebaseio.com/");
                        var reference = db.RootReference;
                        var rankRef = db.GetReference("Game"+game+"_Rank");

                        // 먼저 읽기
                        rankRef.GetValueAsync().ContinueWithOnMainThread(readTask => {
                            if (readTask.IsFaulted) {
                                Debug.LogError("Reading error: " + readTask.Exception);
                                return;
                            }

                            if (readTask.IsCompleted) {
                                DataSnapshot snapshot = readTask.Result;
                                
                                bool nameExists = false;
                                string existingKey = null;
                                string existingScore = "0.0";
                                // 여기서 snapshot으로부터 기존 데이터 확인
                                foreach (DataSnapshot data in snapshot.Children)
                                {
                                    if (data.Value is Dictionary<string, object> rank)
                                    {
                                        string existingName = rank["name"].ToString();
                                        existingScore = rank["score"].ToString();
                                        if (existingName == id)
                                        {
                                            // 해당 이름이 이미 존재
                                            nameExists = true;
                                            existingKey = data.Key;
                                            Debug.Log("이미 존재하는 사용자: " + existingName + ", 현재 점수: " + existingScore);
                                            break;

                                        }
                                    }
                                }

                                // 만약 기존 사용자가 있다면 점수만 업데이트
                                if (nameExists && existingKey != null) {
                                    if (high_score_good && float.Parse(existingScore) <= float.Parse(score))
                                    {
                                        // 새로운 점수로 업데이트
                                        // 단순히 score만 업데이트하고 싶다면 Child("score").SetValueAsync(score)로 가능
                                        reference.Child("Game"+game+"_Rank").Child(existingKey).Child("score").SetValueAsync(score)
                                            .ContinueWithOnMainThread(writeTask => {
                                                if (writeTask.IsFaulted) {
                                                    Debug.LogError("Failed to update data: " + writeTask.Exception);
                                                } else if (writeTask.IsCompleted) {
                                                    Debug.Log("Score successfully updated for existing user!");
                                                    GameManager.Instance.ChangeScene(Scenes.MainMenu,LoadSceneMode.Single);
                                                }
                                            });
                                    }
                                    else if (!high_score_good && float.Parse(existingScore) >= float.Parse(score))
                                    {
                                        // 새로운 점수로 업데이트
                                        // 단순히 score만 업데이트하고 싶다면 Child("score").SetValueAsync(score)로 가능
                                        reference.Child("Game"+game+"_Rank").Child(existingKey).Child("score").SetValueAsync(score)
                                            .ContinueWithOnMainThread(writeTask => {
                                                if (writeTask.IsFaulted) {
                                                    Debug.LogError("Failed to update data: " + writeTask.Exception);
                                                } else if (writeTask.IsCompleted) {
                                                    Debug.Log("Score successfully updated for existing user!");
                                                    GameManager.Instance.ChangeScene(Scenes.MainMenu,LoadSceneMode.Single);
                                                }
                                            });
                                    }
                                    GameManager.Instance.ChangeScene(Scenes.MainMenu,LoadSceneMode.Single);
                                }
                                else {
                                    // 기존 사용자가 없다면 새로 데이터 추가
                                    Rank newRank = new Rank(id, score);
                                    string json = JsonUtility.ToJson(newRank);
                                    string key = reference.Child("Game"+game+"_Rank").Push().Key;

                                    reference.Child("Game"+game+"_Rank").Child(key).SetRawJsonValueAsync(json)
                                        .ContinueWithOnMainThread(writeTask => {
                                            if (writeTask.IsFaulted) {
                                                Debug.LogError("Failed to write data: " + writeTask.Exception);
                                            } else if (writeTask.IsCompleted) {
                                                Debug.Log("Data successfully written!");
                                                GameManager.Instance.ChangeScene(Scenes.MainMenu,LoadSceneMode.Single);
                                            }
                                        });
                                }
                            }
                        });
                    }
                    else
                    {
                        Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                    }
                });
    }
}
