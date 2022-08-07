// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;
using Photon.Pun;

namespace EngineAssembly
{
#pragma warning disable 649

	/// <summary>
	/// Game manager.
	/// Connects and watch Photon Status, Instantiate Player
	/// Deals with quiting the room and the game
	/// Deals with level loading (outside the in room synchronization)
	/// </summary>
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public GameData gameSettings;
		public Transform spawnPoint1;
		public Transform spawnPoint2;
		#region Public Fields

		static public GameManager Instance;

		#endregion

		#region Private Fields

		private GameObject instance;

		[Tooltip("The prefab to use for representing the player")]
		[SerializeField]
		private GameObject senderClientPrefab;
		[SerializeField]
		private GameObject receiverClientPrefab;

		#endregion

		#region MonoBehaviour CallBacks

		static int createdTimes = 0;

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase.
		/// </summary>
		void Start()
		{
			Instance = this;

			// in case we started this demo with the wrong scene being active, simply load the menu scene
			if (!PhotonNetwork.IsConnected)
			{
				SceneManager.LoadScene("Launcher");

				return;
			}


			if (PlayerManager.LocalPlayerInstance == null)
			{
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);


				Vector3 pos = Vector3.zero;
				Quaternion rot = Quaternion.identity;
				switch (PhotonNetwork.CountOfPlayers)
				{
					case 1:
						pos = spawnPoint1.position;
						rot = spawnPoint1.rotation;
						break;
					case 2:
						pos = spawnPoint2.position;
						rot = spawnPoint2.rotation;
						break;
				}

				if (!gameSettings.isReceivingClient)
				{
					createdTimes++;
					PhotonNetwork.Instantiate(this.senderClientPrefab.name, pos, rot, 0);

				}
				else
				{
					createdTimes++;
					PhotonNetwork.Instantiate(this.receiverClientPrefab.name, pos, rot, 0);

				}
				print($"Created Times: {createdTimes}");
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
			}


		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
		void Update()
		{
			// "back" button of phone equals "Escape". quit app if that's pressed
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				QuitApplication();
			}
		}

		#endregion

		#region Photon Callbacks

		/// <summary>
		/// Called when a Photon Player got connected. We need to then load a bigger scene.
		/// </summary>
		/// <param name="other">Other.</param>
		public override void OnPlayerEnteredRoom(Player other)
		{
			Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

				LoadArena();
			}
		}

		/// <summary>
		/// Called when a Photon Player got disconnected. We need to load a smaller scene.
		/// </summary>
		/// <param name="other">Other.</param>
		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

				//LoadArena();
			}
		}

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene("Launcher");
		}

		#endregion

		#region Public Methods

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		public void QuitApplication()
		{
			Application.Quit();
		}

		#endregion

		#region Private Methods

		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			//PhotonNetwork.LoadLevel("GameScene");
		}

		#endregion

	}

}