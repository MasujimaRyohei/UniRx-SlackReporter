using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace UniRx.SlackReport
{
    /// <summary>
    /// Slack reporter.
    /// </summary>
    public static class SlackReporter
    {
        private static string userName = Config.userName;
        private static string webhookUrl = Config.webhookUrl;

        /// <summary>
        /// Posts the text.
        /// </summary>
        /// <param name="input">Input.</param>
        public static void PostText(string input)
        {
            if (webhookUrl == "")
            {
                Debug.LogError("Webhook URL is empty!");
                return;
            }

            PostData data = new PostData
            {
                text = input,
                username = userName
            };

            string json_data = JsonUtility.ToJson(data);

            Observable.FromCoroutine(() => PostCoroutine(webhookUrl, json_data))
                      .Subscribe();
        }

        /// <summary>
        /// Coroutine of posting to slack.
        /// </summary>
        /// <returns>The coroutine.</returns>
        /// <param name="url">URL.</param>
        /// <param name="jsonData">Json data.</param>
        private static IEnumerator PostCoroutine(string url, string jsonData)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();

            header.Add("Content-Type", "application/json; charset=UTF-8");

            byte[] postBytes = Encoding.UTF8.GetBytes(jsonData);

            IDisposable www =
                ObservableWWW.Post(url, postBytes, header)
                             .Retry(3)
                             .Subscribe();

            yield return www;
        }
    }
}