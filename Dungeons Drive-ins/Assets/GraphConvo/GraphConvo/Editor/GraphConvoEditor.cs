using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

namespace GraphConvo
{

    public class GraphConvoEditor : EditorWindow
    {
        ConvoGraphView convoGraphView;
        ConversationContainer conversationContainer;

        [UnityEditor.MenuItem("GraphConvo/Editor")]
        public static GraphConvoEditor Init()
        {
            GraphConvoEditor window = (GraphConvoEditor)EditorWindow.GetWindow(typeof(GraphConvoEditor));
            window.titleContent = new GUIContent("GraphConvoEditor");
            window.Show();
            return window;
        }

        [OnOpenAsset(1)]
        public static bool DoubleClickOpenWindow(int instanceID, int line)
        {
            Object item = EditorUtility.InstanceIDToObject(instanceID);
            if(item is ConversationContainer)
            {
                GraphConvoEditor window = Init();
                window.ConversationContainerChangeCallback(item as ConversationContainer);
            }
            return false;
        }

        private void OnEnable()
        {
            GenerateGraphConvoView();
            GenerateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(convoGraphView);
        }

        private void GenerateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            
            toolbar.Add(new Button(() => SaveData()) { text = "SaveData" });
            toolbar.Add(new Button(() => LoadData()) { text = "LoadData" });

            ObjectField containerObjectField = new ObjectField() { objectType = typeof(ConversationContainer)};
            containerObjectField.name = "containerObjectField";
            containerObjectField.RegisterValueChangedCallback(x => ConversationContainerChangeCallback((ConversationContainer)x.newValue));
            if (conversationContainer != null)
            { 
                ConversationContainerChangeCallback(conversationContainer);
            }

            toolbar.Add(containerObjectField);

            //ToolbarMenu toolbarMenu = new ToolbarMenu();
            //toolbarMenu.text = "Spawn Node";
            //toolbarMenu.menu.AppendAction("Choice", (a) => graphConvoView.SpawnNode(NodeType.Choice, Vector2.zero));
            //toolbarMenu.menu.AppendAction("Dialogue", (a) => graphConvoView.SpawnNode(NodeType.Dialogue, Vector2.zero));

            //toolbar.Add(toolbarMenu);

            rootVisualElement.Add(toolbar);
            
        }

        private void SaveData()
        {
            EditorUtility.SetDirty(conversationContainer);
            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(convoGraphView);
            if (conversationContainer != null)
                saveUtility.SaveGraph(conversationContainer);
            else
                EditorUtility.DisplayDialog("No save file selected.", "No Conversation Container was set, please assign one in the ditor.", "OK");
        }

        private void LoadData()
        {
            GraphSaveUtility saveUtility = GraphSaveUtility.GetInstance(convoGraphView);
            if (conversationContainer != null)
                saveUtility.LoadGraph(conversationContainer);
            else
                EditorUtility.DisplayDialog("No save file selected.", "No Conversation Container was set, please assign one in the ditor.", "OK");
        }

        private void GenerateGraphConvoView()
        {
            convoGraphView = new ConvoGraphView(this)
            {
                name = "Graph Convo View",
            };
            convoGraphView.StretchToParentSize();
            rootVisualElement.Add(convoGraphView);
        }

        private void ConversationContainerChangeCallback(ConversationContainer conversationContainer)
        {
            this.conversationContainer = conversationContainer;
            ObjectField containerObjectField = rootVisualElement.Q<ObjectField>("containerObjectField");
            if (containerObjectField != null) containerObjectField.SetValueWithoutNotify(conversationContainer);
            LoadData();
        }
    }

}
