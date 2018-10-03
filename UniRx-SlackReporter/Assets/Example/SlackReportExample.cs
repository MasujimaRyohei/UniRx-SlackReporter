using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace SlackReport
{
    /// <summary>
    /// Slack report example.
    /// </summary>
    public class SlackReportExample : MonoBehaviour
    {
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private Button submitButton;

        private void Start()
        {
            inputField.OnEndEditAsObservable()
                      .TakeUntilDestroy(this)
                      .ThrottleFirst(TimeSpan.FromSeconds(3.0f))
                      .Subscribe(_ => SubmitToSlack());

            submitButton.OnClickAsObservable()
                        .TakeUntilDestroy(this)
                        .ThrottleFirst(TimeSpan.FromSeconds(3.0f))
                        .Subscribe(_ => SubmitToSlack());
        }

        private void SubmitToSlack()
        {
            SlackReporter.Instance.PostText(inputField.text);
        }
    }
}