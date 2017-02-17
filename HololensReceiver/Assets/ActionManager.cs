using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ActionManager : Singleton<ActionManager>
{
    public enum ActionType { Manipulation, Rotation, Zoom };

    // KeywordRecognizer object.
    KeywordRecognizer keywordRecognizer;

    // Defines which function to call when a keyword is recognized.
    delegate void KeywordAction(PhraseRecognizedEventArgs args);
    Dictionary<string, KeywordAction> keywordCollection;

    public event Action ResetEvent;
    public ActionType CurrentAction { get; private set; }

    void Start()
    {
        keywordCollection = new Dictionary<string, KeywordAction>();

        keywordCollection.Add("Move", MoveCommand);
        keywordCollection.Add("Rotate", RotateCommand);
        keywordCollection.Add("Zoom", ZoomCommand);     
        keywordCollection.Add("Reset", ResetCommand);


        // Initialize KeywordRecognizer with the previously added keywords.
        keywordRecognizer = new KeywordRecognizer(keywordCollection.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        GestureManager.Instance.SetActiveRecognizer(GestureManager.Instance.ManipulationRecognizer);
        CurrentAction = ActionType.Manipulation;
    }

    void OnDestroy()
    {
        if (keywordRecognizer != null)
            keywordRecognizer.Dispose();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        KeywordAction keywordAction;

        if (keywordCollection.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke(args);
        }
    }

    private void MoveCommand(PhraseRecognizedEventArgs args)
    {
        GestureManager.Instance.SetActiveRecognizer(GestureManager.Instance.ManipulationRecognizer);
        CurrentAction = ActionType.Manipulation;
    }

    private void RotateCommand(PhraseRecognizedEventArgs args)
    {
        GestureManager.Instance.SetActiveRecognizer(GestureManager.Instance.NavigationRecognizer);
        CurrentAction = ActionType.Rotation;
    }

    private void ZoomCommand(PhraseRecognizedEventArgs args)
    {
        GestureManager.Instance.SetActiveRecognizer(GestureManager.Instance.NavigationRecognizer);
        CurrentAction = ActionType.Zoom;
    }

    private void ResetCommand(PhraseRecognizedEventArgs args)
    {
        ResetEvent.Invoke();
    }
}
