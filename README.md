# Local LLM Interactive VRM NPC for Unity

In this project, # Local LLM Interactive VRM NPC for Unity

![Project Demo](docs/demo.gif) <!-- Tip: Replace with a GIF showing your VRM NPC responding in-game -->

An interactive Non-Player Character (NPC) framework for Unity that connects 3D **VRM avatars** to a local LLM backend powered by **LM Studio**. This project demonstrates asynchronous network communication in C#, JSON parsing, and integration of third-party open-source Unity packages for real-time AI dialogue.

---

## 🌟 Key Features

* **Local LLM Integration:** Communicates with LM Studio's local OpenAI-compatible API endpoint without requiring external cloud API keys.
* **VRM Avatar Support:** Renders standard `.vrm` character models using the UniVRM framework.
* **Non-Blocking Execution:** Uses C# asynchronous patterns (`async`/`await` and `UnityWebRequest`) to maintain high framerates while waiting for model responses.
* **Prompt & State Management:** Structures dialogue context and system prompts to control NPC personality and responses.

---

## 🏗️ System Architecture

The following diagram illustrates how the game client interacts with the local LLM backend:

```
+-----------------------------------------------------------------------+
|                             Unity Engine                              |
|                                                                       |
|  +--------------------+      HTTP POST      +----------------------+  |
|  |     User Input     |  -----------------> |  LLM Client Service  |  |
|  +--------------------+     (JSON Payload)  +----------------------+  |
|                                                        |              |
|                                                        v              |
|  +--------------------+                    +----------------------+  |
|  | VRM Avatar & UI    | <----------------- | Response Parser      |  |
|  | (UniVRM / Anims)   |    Update Dialogue | (Extracts Message)   |  |
|  +--------------------+    & State         +----------------------+  |
+-----------------------------------------------------------------------+
                                                         |
                                       Async HTTP Req    |   Async HTTP Res
                                       (Port 1234)       |   (Stream / Text)
                                                         v
                                              +--------------------+
                                              |     LM Studio      |
                                              | (Local LLM Server) |
                                              +--------------------+
```

---

## 🛠️ Tech Stack & Prerequisites

* **Engine:** Unity `6.3 LTS`
* **LLM Backend:** [LM Studio](https://lmstudio.ai/) running an OpenAI-compatible local server
* **Third-Party Unity Packages:**
  * [UniVRM](https://github.com/vrm-c/UniVRM) – For importing and displaying VRM avatars
  * [SimpleURPToonLitOutlineExample](https://github.com/simplestargame/SimpleURPToonLitOutlineExample) - For Toon shading

---

## 🚀 Quick Start & Setup

### 1. LM Studio Configuration
1. Open **LM Studio** and download an instruction-tuned model (e.g., Qwen3.5 9B).
2. Navigate to the **Local Server** tab (`<->` icon).
3. Select your model at the top and click **Start Server**.
4. Confirm the server is running on `http://localhost:1234`.

### 2. Unity Project Setup
1. Clone this repository:
   ```bash
   git clone https://github.com/dydxcalibur/UnityLLMNPC.git
   ```
2. Open the project in Unity (`6.3 LTS` or higher).
3. Ensure the **UniVRM** package is properly imported (included in `Packages/` or via UPM).
4. Open the main sample scene:
   `Assets/Scenes/SampleScene.unity`

### 3. Running the Demo
1. Press **Play** in Unity.
2. Type a message in the input field and press `Enter`.
3. View the response returned from LM Studio displayed in the dialogue panel.

---

## ⚙️ Configuration Options

You can adjust the endpoint settings directly on the `LLMLoader` ScriptableObject / Inspector component:

| Setting | Default Value | Description |
| :--- | :--- | :--- |
| **Api Url** | `http://localhost:1234/v1/chat/completions` | Local LM Studio REST endpoint |
| **Model Name** | `local-model` | Target model identifier |
| **Temperature** | `0.7` | Controls output randomness |
| **System Prompt**| *"You are a supportive bartender..."* | Sets the NPC persona |

---

## 💡 Key Learnings & Engineering Challenges

* **Asynchronous Web Requests:** Prevented main-thread freezing during high token generation latencies by wrapping `UnityWebRequest` in asynchronous C# task handlers.
* **Third-Party Package Integration:** Learned how to import, configure, and isolate third-party VRM packages (`UniVRM`) within the Unity package ecosystem.
* **JSON Serialization:** Handled nested data structures for OpenAI-style API payloads using structured C# classes and `JsonUtility` / `Newtonsoft.Json`.

---

## 📜 Credits & Acknowledgments

* **VRM Importer:** Built using [UniVRM](https://github.com/vrm-c/UniVRM) licensed under the MIT License.
* **URP Toon Shader:** Built using [SimpleURPToonLitOutlineExample](https://github.com/simplestargame/SimpleURPToonLitOutlineExample) licensed under the MIT License.
* **Avatar Model:** Modified from Vroid Studio sample model (AvatarSample_M)

---

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.