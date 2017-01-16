using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;
using GooglePlayGames.BasicApi;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class Scene0 : MonoBehaviour, IStoreListener
{
    public GoogleAnalyticsV3 googleAnalytics;

    public Canvas Canvas0, Canvas1, Canvas2, CanvasBeforeIntro, Canvas3, CanvasOpening, CanvasMain, CanvasInventory, CanvasLocation, CanvasMissions, CanvasMissionPrompt, CanvasInMission, CanvasMissionResult, CanvasDiamond, CanvasAbout, CanvasGameOver, CanvasDarbemetre, CanvasBottomBar, CanvasNewsFeed, CanvasNewsFeed2;

    //Canvas0
    public RawImage QuotesandTips;
    public Texture2D[] quotes = new Texture2D[3];
    public Texture2D[] tips = new Texture2D[7];

    //Canvas1
    public GameObject exitPrompt;
    public Button Sound;
    public GameObject continueTheGame;
    public Sprite soundon, soundoff;
    public Text textDiamond, textHighScore;

    //Canvas3
    public GameObject Panel1, Panel2, Panel3, Panel4;
    public AudioClip introSound;

    //CanvasMain
    public Button Soldier, Tank;
    public RawImage Location, Character, Darbemetre, Darbemetrecolor, Mecalmetre;
    public Texture2D[] locations = new Texture2D[6];
    public Texture2D[] characters = new Texture2D[4];
    public Text textDarbe, textName, textLocation, textXP, textSoldier, textTank, textNewsFeed, textDiamond2;

    //CanvasInventory
    public Button slot1, slot2, slotOutput;
    public Text cantabos;
    public Button[] items = new Button[47];
    public Button[] homeitems = new Button[12];
    public Button[] citycenteritems = new Button[16];
    public Button[] pressbuildingitems = new Button[11];
    public Button[] bridgeitems = new Button[10];
    public Button[] airportitems = new Button[14];
    public Button[] militarybaseitems = new Button[12];
    Item itemScript;
    string[] combineInputs = new string[2];
    public GameObject PanelItems;
    public RawImage combineSuccessful;

    //CanvasLocation
    public GameObject promptPanel, Mekanmetre, LocationGorevler, ArkadasDegisim;
    GameObject arkadasdegisimClone;
    public RawImage DarbemetreLocation, MecalmetreLocation;
    public Button[] locationButtons = new Button[6];
    public RawImage[] locationIndicators = new RawImage[6];
    public Texture2D[] inactiveIndicators = new Texture2D[6];
    public Texture2D[] activeIndicators = new Texture2D[6];
    public Text[] arkadasNumbers = new Text[5];
    public Text[] sehitNumbers = new Text[5];
    GameObject Screen0, Screen1, ScreenGorevler, MekanmetreProgress;

    //CanvasMissions
    Mission mission;
    public GameObject[] missionPanels = new GameObject[6];

    //CanvasMissionPrompt
    public GameObject chooseItem, ItemBoxForMission;
    public Text textMissionNamePrompt;
    public Text cantabos2;
    public RawImage singleMission;

    //CanvasInMission
    public GameObject panelNormal, panelFullScreen;

    /*PanelNormal*/
    public RawImage horizontalMission, Gorevmetre, QuotesandTips2;
    public Text textMissionName;

    /*PanelFullScreen*/
    public RawImage fullscrenMission, GorevmetreFullScreen;
    public RawImage circle1, circle2, circle3;
    public Texture2D redCircle, greenCircle;

    //CanvasMissionResult
    public RawImage Canmetre;
    public GameObject gorevBasarisiz;
    public GameObject foundItemsPanel;
    public CanvasGroup sectionMedal, sectionHealth, sectionFriends, sectionSoldier, sectionTank;
    public Text lostCan, gainedMedal, changedHP, gainedfriends, killedSoldier, destroyedTanks, textExplonation, totalXP;
    public RawImage ItemUsed, ItemSuccessful;
    public Texture noitem, successful, fail;
    int randomMedal, randomMecal, randomArkadas, randomAsker, randomTank, XPMission, XPFriend, XPSoldier, XPTank, XPDiamond, XPItem;
    public GameObject[] bulundu;

    //CanvasDiamond
    public Text textDiamond3;

    //CanvasAbout
    public Text sectionone, sectiontwo, sectionthree;
    public Text titleofAbout, contentofAbout, textVersion;
    public GameObject panel1, panel2;

    //CanvasGameOver
    public Text GameOverorWin, gameoverDiamond, gameoverSoldier, gameoverTank, gameoverFriend, gameoverPoint;
    public Texture2D darbebasarili, darbebasarisiz, oldu;

    //CanvasAtesAcildi
    public GameObject atesAcildi;
    public Text atesAcildiText;

    //ScreenTransition
    public GameObject screenTransition;

    //In-App Purchase
    private static IStoreController m_StoreController;

    //Some variables
    InterstitialAd interstitial;
    float timer;
    bool isLocationChanging = false;
    bool isMissionDoing = false;
    Button selected;
    bool isSuitableforItemUsage = false;
    string[] selectedItemName;

    //Raws
    AudioSource audiosystem;
    AudioClip[] missionsounds = new AudioClip[10];
    AudioClip[] combinesounds = new AudioClip[2];

    void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    // Use this for initialization
    void Start()
    {
        //For map screen
        Screen0 = Instantiate(promptPanel) as GameObject;
        Screen1 = Instantiate(Mekanmetre) as GameObject;
        ScreenGorevler = Instantiate(LocationGorevler) as GameObject;
        ScreenGorevler.gameObject.GetComponent<Button>().onClick.AddListener(() => { if (!isLocationChanging) openCanvas(CanvasMissions); });

        //Sounds
        missionsounds[0] = null;
        missionsounds[1] = Resources.Load("GOREV_KAHRAMANTOPLA") as AudioClip;
        missionsounds[2] = Resources.Load("GOREV_SLOGANAT") as AudioClip;
        missionsounds[3] = Resources.Load("GOREV_ASKERIDURDUR") as AudioClip;
        missionsounds[4] = Resources.Load("GOREV_TANKIDURDUR") as AudioClip;
        missionsounds[5] = Resources.Load("OZEL_GOREV_YOLUKAPA") as AudioClip;
        missionsounds[6] = Resources.Load("OZEL_GOREV_HACK") as AudioClip;
        missionsounds[7] = Resources.Load("OZEL_GOREV_SNIPER") as AudioClip;
        missionsounds[8] = Resources.Load("OZEL_GOREV_SISBOMBASI") as AudioClip;
        missionsounds[9] = null;

        combinesounds[0] = Resources.Load("NEWITEM") as AudioClip;
        combinesounds[1] = Resources.Load("ITEMFAIL") as AudioClip;

        audiosystem = GetComponent<AudioSource>();
        initializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanvasLocation.isActiveAndEnabled && isLocationChanging)
        {
            mekanMetre();
        }
        if (CanvasInMission.isActiveAndEnabled && isMissionDoing)
        {
            gorevMetre();
        }
        if (arkadasdegisimClone != null)
        {
            arkadasdegisimClone.transform.position = Vector2.MoveTowards(new Vector2(arkadasdegisimClone.transform.position.x, arkadasdegisimClone.transform.position.y), new Vector2(arkadasdegisimClone.transform.position.x, arkadasdegisimClone.transform.position.y + 100), 3 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CanvasGameOver.isActiveAndEnabled)
            {
                //Do nothing
            }
            else if (CanvasDiamond.isActiveAndEnabled)
            {
                closeCanvas(CanvasDiamond);
            }
            else if (CanvasBeforeIntro.isActiveAndEnabled)
            {
                closeCanvas(CanvasBeforeIntro);
            }
            else if (Canvas2.isActiveAndEnabled)
            {
                closeCanvas(Canvas2);
            }
            else if (CanvasOpening.isActiveAndEnabled)
            {
                closeCanvas(CanvasOpening);
            }
            else if (CanvasAbout.isActiveAndEnabled)
            {
                closeCanvas(CanvasAbout);
            }
            else if (CanvasInventory.isActiveAndEnabled)
            {
                closeCanvas(CanvasInventory);
            }
            else if (CanvasMissionPrompt.isActiveAndEnabled)
            {
                saidNo();
            }
            else if (CanvasMissions.isActiveAndEnabled)
            {
                closeCanvas(CanvasMissions);
            }
            else if (CanvasInMission.isActiveAndEnabled)
            {
                //Do nothing
            }
            else if (CanvasMissionResult.isActiveAndEnabled)
            {
                closeCanvas(CanvasMissionResult);
            }
            else if (CanvasLocation.isActiveAndEnabled)
            {
                closeCanvas(CanvasLocation);
            }
            else if (exitPrompt.activeInHierarchy)
            {
                closeExitPrompt();
            }
            else if (Canvas1.isActiveAndEnabled & !Canvas2.isActiveAndEnabled)
            {
                openExitPrompt();
            }
            else if (CanvasMain.isActiveAndEnabled)
            {
                openCanvas(Canvas1);
                closeCanvas(CanvasMain);
            }
            else
            {
                //Do nothing
            }
        }
    }

    //Called start or restart
    public void initializeGame()
    {
        if (m_StoreController == null)
        {
            InitializeInApp();
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        authenticate();
        checkGameStatus();
        getandSetDiamond();
        checkSoundStatus();
        checkPremium();
        randomQuote();
        if (!checkPremium())
        {
            loadAd();
        }
    }

    //Canvas0
    public void randomQuote()
    {
        int random = Random.Range(0, 3);
        QuotesandTips.GetComponent<RawImage>().texture = quotes[random];

        int random2 = Random.Range(0, 7);
        QuotesandTips2.GetComponent<RawImage>().texture = tips[random2];
    }

    private void authenticate()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                //UserName
                textName.text = Social.localUser.userName;
                PlayerPrefs.SetString("Name", Social.localUser.userName);

                //HighScore
                PlayGamesPlatform.Instance.LoadScores(Constants.leaderboard_en_iyiler, LeaderboardStart.PlayerCentered, 1, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (LeaderboardScoreData data) =>
                {
                    textHighScore.text = data.PlayerScore.formattedValue;
                });
            }
            closeCanvas(Canvas0);
            openCanvas(Canvas1);
        });
    }

    public bool checkPremium()
    {
        bool pre = false;
        string premium = PlayerPrefs.GetString("Premium", "FALSE");
        if (premium == "TRUE")
        {
            pre = true;
        }
        else
        {
            pre = false;
        }

        return pre;
    }


    //Canvas1
    public void checkGameStatus()
    {
        if (getgameStatus() == 1)
        {
            continueTheGame.GetComponent<CanvasGroup>().alpha = 1.0f;
            continueTheGame.GetComponent<CanvasGroup>().interactable = true;
        }
        else
        {
            continueTheGame.GetComponent<CanvasGroup>().alpha = 0.5f;
            continueTheGame.GetComponent<CanvasGroup>().interactable = false;
            PlayerPrefs.SetInt("SU", 1);
            PlayerPrefs.SetInt("BEZ", 1);
            PlayerPrefs.SetInt("TELEFON", 1);
        }
        int loc = PlayerPrefs.GetInt("Location", 0);
        changeLocation(locationButtons[loc].gameObject);
    }

    public void devamEt()
    {
        closeCanvas(Canvas1);
        openCanvas(CanvasMain);
        Invoke("showAd", 5.0f);

        //Arkadaş ve şehit sayısı düzenli artış
        InvokeRepeating("ArkadasandSehit", 5, 5F);

        //Halkgücünü düzenli azalt
        InvokeRepeating("updateMeters", 5, 1F);

        //NewsFeed
        StartCoroutine("newsFeed");

        //AtesAcildi
        StartCoroutine("AtesAcildi");

        darbeMetre();
        getandSetMecal();
    }

    public void yeniOyun()
    {
        if (getgameStatus() == 1)
        {
            restartTheGame();
        }
        else
        {
            openCanvas(Canvas2);
        }
    }

    public void showAchievements()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            authenticate();
        }
    }

    public void showLeaderBoard()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(Constants.leaderboard_en_iyiler);
        }
        else
        {
            authenticate();
        }
    }

    public void openExitPrompt()
    {
        exitPrompt.gameObject.SetActive(true);
    }

    public void closeExitPrompt()
    {
        exitPrompt.gameObject.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void checkSoundStatus()
    {
        string soundStatus = PlayerPrefs.GetString("Sound", "ON");
        if (soundStatus == "OFF")
        {
            AudioListener.volume = 0;
            Sound.GetComponent<Button>().image.overrideSprite = soundoff;
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            AudioListener.volume = 1;
            Sound.GetComponent<Button>().image.overrideSprite = soundon;
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    public void soundSettings()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
            Sound.GetComponent<Button>().image.overrideSprite = soundon;
            PlayerPrefs.SetString("Sound", "ON");
        }
        else
        {
            AudioListener.volume = 0;
            Sound.GetComponent<Button>().image.overrideSprite = soundoff;
            PlayerPrefs.SetString("Sound", "OFF");
        }
    }

    //Canvas2
    public void chooseCharecter(int order)
    {
        if (order == 1)
        {
            PlayerPrefs.SetString("Character", "Male1");
        }
        else if (order == 2)
        {
            PlayerPrefs.SetString("Character", "Male2");
        }
        else if (order == 3)
        {
            PlayerPrefs.SetString("Character", "Female1");
        }
        else
        {
            PlayerPrefs.SetString("Character", "Female2");
        }
        openCanvas(CanvasBeforeIntro);
        setProfileImageandName();
    }

    //CanvasBeforeIntro
    public void chooseDifficulty(int difficulty)
    {
        if (difficulty == 0)
        {
            PlayerPrefs.SetFloat("HalkGucu", 70.0f);
        }
        else if (difficulty == 1)
        {
            PlayerPrefs.SetFloat("HalkGucu", 50.0f);
        }
        else if (difficulty == 2)
        {
            PlayerPrefs.SetFloat("HalkGucu", 30.0f);
        }
        else
        {
            //Do nothing
        }

        float halkgucu = PlayerPrefs.GetFloat("HalkGucu");

        Darbemetre.rectTransform.localScale = new Vector2(halkgucu / 100, Darbemetre.rectTransform.localScale.y);
        DarbemetreLocation.rectTransform.localScale = new Vector2((100.0f - halkgucu) / 100.0f, DarbemetreLocation.rectTransform.localScale.y);

        openCanvas(Canvas3);
        closeCanvas(Canvas1);
        closeCanvas(Canvas2);
        closeCanvas(CanvasBeforeIntro);
        StartCoroutine(Intro());
    }


    public IEnumerator Intro()
    {
        yield return new WaitForSeconds(5);
        Instantiate(screenTransition);
        yield return new WaitForSeconds(0.375f);
        Panel1.gameObject.SetActive(false);
        Panel2.gameObject.SetActive(true);
        audiosystem.PlayOneShot(introSound, 1.0F);
        yield return new WaitForSeconds(4.0f);
        Instantiate(screenTransition);
        yield return new WaitForSeconds(0.375f);
        Panel2.gameObject.SetActive(false);
        Panel3.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        Instantiate(screenTransition);
        yield return new WaitForSeconds(0.375f);
        Panel3.gameObject.SetActive(false);
        Panel4.gameObject.SetActive(true);
    }

    //Canvas3
    public void skipIntro()
    {
        closeCanvas(Canvas3);
        openCanvas(CanvasMain);
        openCanvas(CanvasOpening);

        audiosystem.Stop();

        //Arkadaş ve şehit sayısı düzenli artış
        InvokeRepeating("ArkadasandSehit", 15, 5F);

        //Halkgücünü düzenli azalt
        InvokeRepeating("updateMeters", 15, 1F);

        //NewsFeed
        StartCoroutine("newsFeed");

        //AtesAcildi
        StartCoroutine("AtesAcildi");
    }

    /* CanvasMain */
    public void darbeMetre()
    {
        float halkgucu = getHalkGucu();
        if (halkgucu <= 0.0f)
        {
            CancelInvoke();
            StopAllCoroutines();
            PlayerPrefs.SetFloat("HalkGucu", 0.0f);
            PlayerPrefs.SetInt("Status", 2);
            getgameStatus();
        }
        else if (halkgucu >= 100.0f)
        {
            CancelInvoke();
            StopAllCoroutines();
            PlayerPrefs.SetFloat("HalkGucu", 100.0f);
            PlayerPrefs.SetInt("Status", 3);
            getgameStatus();
        }
        else
        {
            Debug.logger.Log(halkgucu);
            PlayerPrefs.SetFloat("HalkGucu", halkgucu);
            Darbemetre.rectTransform.localScale = new Vector2(halkgucu / 100, Darbemetre.rectTransform.localScale.y);
            DarbemetreLocation.rectTransform.localScale = new Vector2((100.0f - halkgucu) / 100.0f, DarbemetreLocation.rectTransform.localScale.y);
        }
    }

    public float getandSetMecal()
    {
        float mecal = PlayerPrefs.GetFloat("Mecal", 100.0f);
        if (mecal <= 0.0f)
        {
            mecal = 0.0f;
            PlayerPrefs.SetInt("Status", 4);
            getgameStatus();
        }
        else if (mecal >= 100.0f)
        {
            mecal = 100.0f;
        }
        else
        {

        }
        Mecalmetre.rectTransform.localScale = new Vector3(mecal / 100.0f, Mecalmetre.rectTransform.localScale.y);
        MecalmetreLocation.rectTransform.localScale = new Vector3(mecal / 100.0f, MecalmetreLocation.rectTransform.localScale.y);
        PlayerPrefs.SetFloat("Mecal", mecal);
        return mecal;
    }

    public void updateMeters()
    {
        PlayerPrefs.SetFloat("HalkGucu", getHalkGucu() - 0.1f);
        darbeMetre();
    }

    public IEnumerator newsFeed()
    {
        string[] news = { "22.05 Ankara'da savaş uçakları alçak uçuş yapıyor", "22.10 Türkiye çapında asker-polis karşı karşıya geldi!", "22.30 Genelkurmay'dan silah sesleri geliyor!", "22.35 Tanklar Atatürk Havalimanı'da!", "23.10 Başbakan Binali Yıldırım: Bu bir kalkışmadır.", "23.24 Gölbaşı Polis Özel Harekat Eğitim Merkezi'nde patlama", "23.25 Sosyal medyaya erişim engeli geldi!", "23.30  Genelkurmay Başkanı Orgeneral Hulusi Akar rehin alındı", "23.36: Başbakan: Ucunda ölüm dahi olsa gereken yapılacak.", "23.45 Darbeciler, TSK adıyla, yönetime el koyduklarını açıkladı.", "23.50 Ankara'nın dört bir yanında patlama sesleri...", "00.06 Milli Savunma Bakanı: TSK bildirisi korsan bildiridir.", "00.15 Darbeciler TRT spikerine zorla bildiri okuttu.", "00.37 Cumhurbaşkanı Erdoğan: Halk sokağa çıksın!", "00.40 Asker, Atatürk Havalimanı kontrol kulesine el koydu.", "00.48 Ankara'da vatandaşlar, tankların üzerine çıktı.", "01.10 Askeri helikopter TÜRKSAT uydu istasyonunu vurdu.", "01.22 Camilerden ezan okunmaya başlandı.", "01.28: Deniz Kuvvetleri: Bu girişimi kesinlikle kabul etmiyoruz.", "01.40 Darbeci askerler Boğaz Köprüsü'nde halka ateş açtı.", "01.50: Özel Kuvvetler Komutanı: Başarılı olamayacaklar!", "02.09 3. Kolordu Komutanı: Asker derhal kışlalarına dönmeli", "02.13 Gölbaşı Özel Harekat Merkezi'nde 17 polis şehit oldu", "02.21 Polisleri şehit eden helikopter Gölbaşı'nda düşürüldü.", "02.25 Asker Taksim'de halkı dağıtmak için havaya ateş açtı.", "02.30 MİT Basın Danışmanı Nuh Yılmaz: Darbe püskürtüldü.", "02.42 TBMM'ye atılan bomba nedeniyle yaralılar var", "02.50 F16'lar ve helikopterler TBMM'yi vurmaya başladı.", "02.57 TÜRKSAT Kampüsü'nde görevli 2 personel şehit oldu.", "03.12 Genelkurmay'dan silah sesleri gelmeye başladı.", "03.22 Ankara Gölbaşı'ndaki TÜRKSAT kampüsü bombalandı.", "03.23 Darbeciler Hürriyet ve CNNTürk binasına girdi.", "03.40 Genelkurmay önünde toplanan vatandaşlara ateş açıldı.", "04.07 Cumhurbaşkanı: Milletin üzerinde hiçbir güç yoktur.", "04.37 Cumhurbaşkanı: Bunun bedelini çok ağır ödeyecekler.", "05.02 Cumhurbaşkanlığı: Tehlike henüz geçmiş değil.", "06.00 Şu ana kadar 160'ı aşkın vatandaş hayatını kaybetti.", "06.26 Cumhurbaşkanlığı'ndan dumanlar yükseliyor." };

        for (int i = 0; i < 38; i++)
        {
            textNewsFeed.text = news[i];
            yield return new WaitForSeconds(8);
        }
        StopCoroutine(newsFeed());
        StartCoroutine(newsFeed());
    }

    /* CanvasInventory */
    public void updateInventory()
    {
        for (int i = 0; i < items.Length; i++)
        {
            itemScript = items[i].gameObject.GetComponent(typeof(Item)) as Item;
            int amount = PlayerPrefs.GetInt(itemScript.itemName);

            if (amount > 0)
            {
                //Inventory
                Button item = Instantiate(items[i]) as Button;
                item.transform.SetParent(PanelItems.transform, false);
                item.GetComponent<Button>().onClick.AddListener(() => { selectItemforCombine(item); });
                item.GetComponentInChildren<Text>().text = amount.ToString();
                cantabos.gameObject.SetActive(false);
            }
        }
    }

    public void selectItemforCombine(Button btn)
    {
        if (slot1.interactable)
        {
            slot1.gameObject.GetComponent<RawImage>().texture = btn.gameObject.GetComponent<RawImage>().texture;
            slot1.interactable = false;
            combineInputs[0] = btn.gameObject.name;
        }
        else if (slot2.interactable && !slot1.interactable)
        {
            slot2.gameObject.GetComponent<RawImage>().texture = btn.gameObject.GetComponent<RawImage>().texture;
            slot2.interactable = false;
            combineInputs[1] = btn.gameObject.name;
            doCombine();
        }
        else
        {
            StartCoroutine(clearSlots());
        }
    }

    public void doCombine()
    {
        int c = 0;

        if (combineInputs.Contains("SU(Clone)") && combineInputs.Contains("VARIL(Clone)"))
        {
            c = PlayerPrefs.GetInt("VARILYARIM") + 1;
            PlayerPrefs.SetInt("VARILYARIM", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[28].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("SU(Clone)") && combineInputs.Contains("VARILYARIM(Clone)"))
        {
            c = PlayerPrefs.GetInt("VARILTAM") + 1;
            PlayerPrefs.SetInt("VARILTAM", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[29].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BENZIN(Clone)") && combineInputs.Contains("VARIL(Clone)"))
        {
            c = PlayerPrefs.GetInt("VARILYARIMBENZIN") + 1;
            PlayerPrefs.SetInt("VARILYARIMBENZIN", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[30].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BENZIN(Clone)") && combineInputs.Contains("VARILYARIMBENZIN(Clone)"))
        {
            c = PlayerPrefs.GetInt("VARILTAMBENZIN") + 1;
            PlayerPrefs.SetInt("VARILTAMBENZIN", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[31].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BENZIN(Clone)") && combineInputs.Contains("HILTI(Clone)"))
        {
            c = PlayerPrefs.GetInt("HILTIYARIM") + 1;
            PlayerPrefs.SetInt("HILTIYARIM", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[26].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BENZIN(Clone)") && combineInputs.Contains("HILTIYARIM(Clone)"))
        {
            c = PlayerPrefs.GetInt("HILTITAM") + 1;
            PlayerPrefs.SetInt("HILTITAM", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[27].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BENZIN(Clone)") && combineInputs.Contains("DETERJAN(Clone)"))
        {
            c = PlayerPrefs.GetInt("KIMYASAL") + 1;
            PlayerPrefs.SetInt("KIMYASAL", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[32].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("TELSIZ(Clone)") && combineInputs.Contains("TORNAVIDA(Clone)"))
        {
            c = PlayerPrefs.GetInt("CIP") + 1;
            PlayerPrefs.SetInt("CIP", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[33].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("ARABA(Clone)") && combineInputs.Contains("MEGAFON(Clone)"))
        {
            c = PlayerPrefs.GetInt("ARABAMEGAFONLU") + 1;
            PlayerPrefs.SetInt("ARABAMEGAFONLU", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[34].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BILGISAYAR(Clone)") && combineInputs.Contains("TELEFON(Clone)"))
        {
            c = PlayerPrefs.GetInt("BILGISAYARINTERNETLI") + 1;
            PlayerPrefs.SetInt("BILGISAYARINTERNETLI", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[35].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BEZ(Clone)") && combineInputs.Contains("SU(Clone)"))
        {
            c = PlayerPrefs.GetInt("ISLAKBEZ") + 1;
            PlayerPrefs.SetInt("ISLAKBEZ", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[36].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("ARABAMEGAFONLU(Clone)") && combineInputs.Contains("BENZIN(Clone)"))
        {
            c = PlayerPrefs.GetInt("HOH") + 1;
            PlayerPrefs.SetInt("HOH", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[37].gameObject.GetComponent<RawImage>().texture;

            Social.ReportProgress(Constants.achievement_hh, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        else if (combineInputs.Contains("KAMERA(Clone)") && combineInputs.Contains("BILGISAYARINTERNETLI(Clone)"))
        {
            c = PlayerPrefs.GetInt("YAYINGUCU") + 1;
            PlayerPrefs.SetInt("YAYINGUCU", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[38].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("KIMYASAL(Clone)") && combineInputs.Contains("METALKUTU(Clone)"))
        {
            c = PlayerPrefs.GetInt("GAZBOMBASIHAZIRLIK") + 1;
            PlayerPrefs.SetInt("GAZBOMBASIHAZIRLIK", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[39].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("KIMYASAL(Clone)") && combineInputs.Contains("VARILTAMBENZIN(Clone)"))
        {
            c = PlayerPrefs.GetInt("TEHLIKELIMADDE") + 1;
            PlayerPrefs.SetInt("TEHLIKELIMADDE", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[40].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("CIP(Clone)") && combineInputs.Contains("PIL(Clone)"))
        {
            c = PlayerPrefs.GetInt("LAZER") + 1;
            PlayerPrefs.SetInt("LAZER", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[41].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("CIP(Clone)") && combineInputs.Contains("TORNAVIDA(Clone)"))
        {
            c = PlayerPrefs.GetInt("LAZER") + 1;
            PlayerPrefs.SetInt("SIFREKIRICI", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[42].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("LAZER(Clone)") && combineInputs.Contains("MAKINELITUFEK(Clone)"))
        {
            c = PlayerPrefs.GetInt("SNIPERTUFEGI") + 1;
            PlayerPrefs.SetInt("SNIPERTUFEGI", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[43].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("GAZBOMBASIHAZIRLIK(Clone)") && combineInputs.Contains("ASIT(Clone)"))
        {
            c = PlayerPrefs.GetInt("GAZBOMBASI") + 1;
            PlayerPrefs.SetInt("GAZBOMBASI", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[44].gameObject.GetComponent<RawImage>().texture;

            Social.ReportProgress(Constants.achievement_kimyager, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        else if (combineInputs.Contains("TEHLIKELIMADDE(Clone)") && combineInputs.Contains("IP(Clone)"))
        {
            c = PlayerPrefs.GetInt("PATLAYICIVARIL") + 1;
            PlayerPrefs.SetInt("PATLAYICIVARIL", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[45].gameObject.GetComponent<RawImage>().texture;
        }
        else if (combineInputs.Contains("BILGISAYARINTERNETLI(Clone)") && combineInputs.Contains("SIFREKIRICI(Clone)"))
        {
            c = PlayerPrefs.GetInt("HACKYETENEGI") + 1;
            PlayerPrefs.SetInt("HACKYETENEGI", c);
            slotOutput.gameObject.GetComponent<RawImage>().texture = items[46].gameObject.GetComponent<RawImage>().texture;
        }
        /* YANMALI COMBINELAR */
        else if (combineInputs.Contains("TELEFON(Clone)") && combineInputs.Contains("SU(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("BENZIN(Clone)") && combineInputs.Contains("CAKMAK(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("BILGISAYAR(Clone)") && combineInputs.Contains("SU(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("BILGISAYARINTERNETLI(Clone)") && combineInputs.Contains("SU(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("TELSIZ(Clone)") && combineInputs.Contains("SU(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("KAMERA(Clone)") && combineInputs.Contains("SU(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("LAZER(Clone)") && combineInputs.Contains("SU(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("KAMERA(Clone)") && combineInputs.Contains("BENZIN(Clone)"))
        {
            c = -1;
        }
        else if (combineInputs.Contains("CAKMAK(Clone)") && combineInputs.Contains("BEZ(Clone)"))
        {
            c = -1;
        }
        else
        {
            //Do nothing
        }

        //If combine successfull
        if (c > 0)
        {
            string[] arraya = combineInputs[0].Split('(');
            string[] arrayb = combineInputs[1].Split('(');
            int a = PlayerPrefs.GetInt(arraya[0]);
            int b = PlayerPrefs.GetInt(arrayb[0]);

            PlayerPrefs.SetInt(arraya[0], a - 1);
            PlayerPrefs.SetInt(arrayb[0], b - 1);

            combineSuccessful.gameObject.SetActive(true);
            combineSuccessful.texture = successful;
            audiosystem.PlayOneShot(combinesounds[0], 1.0F);
        }
        else if (c == 0)
        {
            combineSuccessful.gameObject.SetActive(true);
            combineSuccessful.texture = fail;
        }
        else
        {
            string[] arraya = combineInputs[0].Split('(');
            string[] arrayb = combineInputs[1].Split('(');
            int a = PlayerPrefs.GetInt(arraya[0]);
            int b = PlayerPrefs.GetInt(arrayb[0]);

            PlayerPrefs.SetInt(arraya[0], a - 1);
            PlayerPrefs.SetInt(arrayb[0], b - 1);

            combineSuccessful.gameObject.SetActive(true);
            combineSuccessful.texture = fail;
            audiosystem.PlayOneShot(combinesounds[1], 1.0F);
        }

        //Whether success or fail clear the slots after 1.75f
        c = 0;
        StartCoroutine(clearSlots());
    }

    public IEnumerator clearSlots()
    {
        yield return new WaitForSeconds(1.5f);
        slot1.gameObject.GetComponent<RawImage>().texture = null;
        slot2.gameObject.GetComponent<RawImage>().texture = null;
        slotOutput.gameObject.GetComponent<RawImage>().texture = null;

        slot1.interactable = true;
        slot2.interactable = true;

        combineSuccessful.gameObject.SetActive(false);
        combineSuccessful.texture = null;
        combineInputs[0] = null;
        combineInputs[1] = null;
        clearInventory();
        updateInventory();
        StopCoroutine(clearSlots());
    }

    public void clearInventory()
    {
        cantabos.gameObject.SetActive(true);
        foreach (Transform t in PanelItems.transform)
        {
            Destroy(t.gameObject);
        }
    }

    //CanvasLocation
    public void ArkadasandSehit()
    {
        int randomArkadas = 0;
        int randomSehit = 0;
        int totalArkadas = PlayerPrefs.GetInt("TotalArkadas", 0);

        for (int i = 0; i < 5; i++)
        {
            int arkadas = PlayerPrefs.GetInt("arkadas" + i, i);
            int sehit = PlayerPrefs.GetInt("sehit" + i, i);

            randomArkadas = Random.Range(2, 6);
            randomSehit = Random.Range(0, 2);

            arkadasNumbers[i].text = (arkadas + randomArkadas).ToString();
            sehitNumbers[i].text = (sehit + randomSehit).ToString();

            PlayerPrefs.SetInt("arkadas" + i, arkadas + randomArkadas);
            PlayerPrefs.SetInt("sehit" + i, sehit + randomSehit);
            PlayerPrefs.SetInt("TotalArkadas", totalArkadas + arkadas + randomArkadas);

            StartCoroutine(wait());
        }
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void changeLocation(GameObject loc)
    {
        switch (loc.name)
        {
            case "Home":
                PlayerPrefs.SetInt("Location", 0);
                textLocation.text = "[EV]";
                break;
            case "CityCenter":
                PlayerPrefs.SetInt("Location", 1);
                textLocation.text = "[ŞEHİR MERKEZİ]";
                break;
            case "PressBuilding":
                PlayerPrefs.SetInt("Location", 2);
                textLocation.text = "[BASIN BİNASI]";
                break;
            case "Bridge":
                PlayerPrefs.SetInt("Location", 3);
                textLocation.text = "[KÖPRÜ]";
                break;
            case "Airport":
                PlayerPrefs.SetInt("Location", 4);
                textLocation.text = "[HAVAALANI]";
                break;
            case "MilitaryBase":
                PlayerPrefs.SetInt("Location", 5);
                textLocation.text = "[ASKERİ ÜS]";
                break;
            default:
                break;
        }

        for (int i = 0; i <= 5; i++)
        {
            locationButtons[i].interactable = true;
            locationIndicators[i].texture = inactiveIndicators[i];
            missionPanels[i].gameObject.SetActive(false);
        }

        ScreenGorevler.transform.SetParent(loc.transform, false);

        int id = PlayerPrefs.GetInt("Location");
        locationButtons[id].interactable = false;
        locationIndicators[id].texture = activeIndicators[id];
        missionPanels[id].gameObject.SetActive(true);
        Location.texture = locations[id];
    }

    public void locationPrompt(GameObject go)
    {
        if (!isLocationChanging)
        {
            Screen0.gameObject.SetActive(true);
            Screen0.transform.SetParent(go.transform, false);
            foreach (Transform t in Screen0.transform)
            {
                if (t.name == "Yes")
                {
                    t.gameObject.GetComponent<Button>().onClick.AddListener(() => { changingLocation(go); });
                }
                else if (t.name == "No")
                {
                    t.gameObject.GetComponent<Button>().onClick.AddListener(() => { closeLocationPrompt(); });
                }
            }
        }
    }

    public void changingLocation(GameObject go)
    {
        Screen0.transform.SetParent(null, false);
        Screen1.transform.SetParent(go.transform, false);
        isLocationChanging = true;
        foreach (Transform t in Screen1.transform)
        {
            if (t.name == "MekanmetreProgress")
            {
                MekanmetreProgress = t.gameObject;
            }
        }
    }

    public void closeLocationPrompt()
    {
        Screen0.transform.SetParent(null, false);
    }

    public void mekanMetre()
    {
        if (timer < 3.0f)
        {
            timer += Time.deltaTime;
            MekanmetreProgress.GetComponent<RawImage>().rectTransform.localScale = new Vector3(timer / 3.0f, MekanmetreProgress.GetComponent<RawImage>().rectTransform.localScale.y);
        }
        else
        {
            changeLocation(Screen1.transform.parent.gameObject);
            Screen1.transform.SetParent(null, false);
            isLocationChanging = false;
            timer = 0.0f;
            openCanvas(CanvasMissions);
        }
    }

    /* CanvasMission */
    public void canDoldur()
    {
        if (getandSetDiamond() >= 5)
        {
            if (getandSetMecal() != 100.0f && getandSetMecal() != 0.0f)
            {
                PlayerPrefs.SetFloat("Mecal", 100.0f);
                PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() - 5);
                getandSetDiamond();
                getandSetMecal();
                closeCanvas(CanvasMissions);
            }
            else
            {

            }
        }
        else
        {
            openCanvas(CanvasDiamond);
        }
    }

    public void chooseMission(GameObject go)
    {
        if (go.name == "MissionSpecialCityCenter")
        {
            if (getandSetDiamond() >= 5)
            {
                missionPrompt(go);
                openCanvas(CanvasMissionPrompt);
                closeCanvas(CanvasMissions);
            }
            else
            {
                openCanvas(CanvasDiamond);
            }
        }
        else if (go.name == "MissionSpecialPressBuilding")
        {
            if (getandSetDiamond() >= 10)
            {
                missionPrompt(go);
                openCanvas(CanvasMissionPrompt);
                closeCanvas(CanvasMissions);
            }
            else
            {
                openCanvas(CanvasDiamond);
            }
        }
        else if (go.name == "MissionSpecialBridge")
        {
            if (getandSetDiamond() >= 15)
            {
                missionPrompt(go);
                openCanvas(CanvasMissionPrompt);
                closeCanvas(CanvasMissions);
            }
            else
            {
                openCanvas(CanvasDiamond);
            }
        }
        else if (go.name == "MissionSpecialAirport")
        {
            if (getandSetDiamond() >= 20)
            {
                missionPrompt(go);
                openCanvas(CanvasMissionPrompt);
                closeCanvas(CanvasMissions);
            }
            else
            {
                openCanvas(CanvasDiamond);
            }
        }
        else if (go.name == "MissionSpecialMilitaryBase")
        {
            if (getandSetDiamond() >= 25)
            {
                missionPrompt(go);
                openCanvas(CanvasMissionPrompt);
                closeCanvas(CanvasMissions);
            }
            else
            {
                openCanvas(CanvasDiamond);
            }
        }
        else
        {
            missionPrompt(go);
            openCanvas(CanvasMissionPrompt);
            closeCanvas(CanvasMissions);
        }
    }

    //CanvasMissionPrompt
    public void missionPrompt(GameObject go)
    {
        mission = go.GetComponent(typeof(Mission)) as Mission;
        textMissionNamePrompt.text = mission.MissionName;
        singleMission.texture = mission.MissionPrompt;

        if (mission.MissionName == "GÖREV YAPILIYOR: ÇEVREYİ ARA")
        {
            isSuitableforItemUsage = false;
            chooseItem.gameObject.SetActive(false);
        }
        else
        {
            isSuitableforItemUsage = true;
            chooseItem.gameObject.SetActive(true);
        }
        updateInventoryforMission();
    }

    public void updateInventoryforMission()
    {
        for (int i = 0; i < items.Length; i++)
        {
            itemScript = items[i].gameObject.GetComponent(typeof(Item)) as Item;
            int amount = PlayerPrefs.GetInt(itemScript.itemName);
            if (amount > 0)
            {
                if (isSuitableforItemUsage)
                {
                    Button item = Instantiate(items[i]) as Button;
                    item.transform.SetParent(ItemBoxForMission.transform, false);
                    item.GetComponent<Button>().onClick.AddListener(() => { selectItemforMission(item); });
                    item.gameObject.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);
                    item.GetComponentInChildren<Text>().text = amount.ToString();
                    cantabos2.gameObject.SetActive(false);
                }
            }
        }
    }

    public void selectItemforMission(Button btn)
    {
        if (selected == null)
        {
            btn.gameObject.GetComponent<RawImage>().color = new Color(1, 1, 1, 1.0f);
            ItemUsed.texture = btn.gameObject.GetComponent<RawImage>().texture;
            selectedItemName = btn.name.Split('(');
            PlayerPrefs.SetString("USEDITEM", selectedItemName[0]);
            selected = btn;
        }
        else
        {
            if (btn == selected)
            {
                btn.gameObject.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);
                ItemUsed.texture = noitem;
                PlayerPrefs.SetString("USEDITEM", null);
                selected = null;
            }
            else
            {
                selected.gameObject.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);

                btn.gameObject.GetComponent<RawImage>().color = new Color(1, 1, 1, 1.0f);
                ItemUsed.texture = btn.gameObject.GetComponent<RawImage>().texture;
                selectedItemName = btn.name.Split('(');
                PlayerPrefs.SetString("USEDITEM", selectedItemName[0]);
                selected = btn;
            }
        }
    }

    public void saidYes()
    {
        closeCanvas(CanvasMissionPrompt);
        openCanvas(CanvasInMission);
        textMissionName.text = mission.MissionName;
        randomQuote();
        playSoundInMission();
        isMissionDoing = true;
        StartCoroutine(resetMissionPrompt());
    }

    public void saidNo()
    {
        ItemUsed.texture = noitem;
        PlayerPrefs.SetString("USEDITEM", null);
        closeCanvas(CanvasMissionPrompt);
        openCanvas(CanvasMissions);
        StartCoroutine(resetMissionPrompt());
    }

    public IEnumerator resetMissionPrompt()
    {
        yield return new WaitForSeconds(0.375f);
        foreach (Transform t in ItemBoxForMission.transform)
        {
            Destroy(t.gameObject);
        }
        cantabos2.gameObject.SetActive(true);
    }

    //CanvasInMission
    public void playSoundInMission()
    {
        if (mission.MissionName == "GÖREV YAPILIYOR: ÇEVREYİ ARA")
        {
            //Not exist
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: KAHRAMAN TOPLA")
        {
            audiosystem.PlayOneShot(missionsounds[1], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: SLOGAN AT")
        {
            audiosystem.PlayOneShot(missionsounds[2], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: ASKERİ DURDUR")
        {
            audiosystem.PlayOneShot(missionsounds[3], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: TANKI DURDUR")
        {
            audiosystem.PlayOneShot(missionsounds[4], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: YOLU ARAÇ TRAFİĞİNE KAPA")
        {
            audiosystem.PlayOneShot(missionsounds[5], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: YAYINI ÖZGÜRLEŞTİR")
        {
            audiosystem.PlayOneShot(missionsounds[6], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: KESKİN NİŞANCIYI ETKİSİZ HALE GETİR")
        {
            audiosystem.PlayOneShot(missionsounds[7], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: KONTROL KULESİNİ ELE GEÇİR")
        {
            audiosystem.PlayOneShot(missionsounds[8], 1.0F);
        }
        else if (mission.MissionName == "GÖREV YAPILIYOR: MASUM ASKERLERİ KURTAR")
        {
            //Not exist
        }
        else
        {
            //Do nothing
        }
    }

    public void gorevMetre()
    {
        float MissionTime = mission.MissionTime;
        if (timer < MissionTime)
        {
            timer += Time.deltaTime;
            if (mission.FullScreenMission == null)
            {
                Gorevmetre.rectTransform.localScale = new Vector3(timer / MissionTime, Gorevmetre.rectTransform.localScale.y);

            }
            else
            {
                GorevmetreFullScreen.rectTransform.localScale = new Vector3(timer / MissionTime, GorevmetreFullScreen.rectTransform.localScale.y);
                circle1.transform.Rotate(90 * Vector3.back * Time.deltaTime);
                circle2.transform.Rotate(90 * Vector3.forward * Time.deltaTime);
                circle3.transform.Rotate(90 * Vector3.back * Time.deltaTime);
            }
        }
        else
        {
            isMissionDoing = false;
            timer = 0.0f;
            evaluateMission(mission.MissionName);
            closeCanvas(CanvasInMission);
            openCanvas(CanvasMissionResult);
        }
    }

    public void clickClick(GameObject go)
    {
        StartCoroutine(clickClickClick(go));
    }

    public IEnumerator clickClickClick(GameObject go)
    {
        timer += 0.5f;
        go.gameObject.GetComponent<RawImage>().texture = redCircle;
        go.gameObject.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(0.25f);
        go.gameObject.GetComponent<RawImage>().texture = greenCircle;
        go.gameObject.GetComponent<Button>().interactable = true;
    }

    //CanvasMissionResult
    public void evaluateMission(string name)
    {
        int medal = getandSetDiamond();
        float mecal = getandSetMecal();
        int asker = getandSetSoldier();
        int tank = getandSetTank();
        int XP = PlayerPrefs.GetInt("XP");
        string itemwhicused = PlayerPrefs.GetString("USEDITEM", null);

        if (name == "GÖREV YAPILIYOR: ÇEVREYİ ARA")
        {
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(findItem());
            }
        }
        else if (name == "GÖREV YAPILIYOR: KAHRAMAN TOPLA")
        {
            if (itemwhicused != null)
            {
                StartCoroutine(findItem());
                switch (itemwhicused)
                {
                    case "HOH":
                        randomArkadas = Random.Range(14, 18);
                        textExplonation.text = "Halk Özel Harekat güçleri sayesinde pek çok kahraman topladın.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "YAYINGUCU":
                        randomArkadas = Random.Range(14, 18);
                        textExplonation.text = "Yayın gücü sayesinde millete gerçekleri anlattın. Millet meydanlarda!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "BILGISAYARINTERNETLI":
                        randomArkadas = Random.Range(10, 14);
                        textExplonation.text = "Sosyal medya paylaşımlarınla birçok kişiye eriştin ve onların meydanlara çıkmasını sağladın";
                        ItemSuccessful.texture = successful;
                        break;
                    case "ARABAMEGAFONLU":
                        randomArkadas = Random.Range(10, 14);
                        textExplonation.text = "Megafonlu araba sayesinde, hâlâ evinde oturanları haberdar ettin ve darbeye karşı sokağa çıkmaya çağırdın.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "MEGAFON":
                        randomArkadas = Random.Range(6, 10);
                        textExplonation.text = "Megafon kullanarak daha geniş kitleleri darbeye karşı sokağa çıkmaya çağırdın.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "TELEFON":
                        randomArkadas = Random.Range(3, 6);
                        textExplonation.text = "Telefonla arkadaşlarına, akrabalarına haber vererek onların darbe girişiminden haberdar olmalarını ve sokağa çıkmalarını sağladın";
                        ItemSuccessful.texture = successful;
                        break;
                    case "BAYRAK":
                        randomArkadas = Random.Range(3, 6);
                        textExplonation.text = "Bayrak kullanarak insanların ilgisini çekmeyi başardın ve onların darbe girişiminden haberdar olmalarını sağladın";
                        ItemSuccessful.texture = successful;
                        break;
                    default:
                        randomArkadas = Random.Range(1, 3);
                        textExplonation.text = "Kullandığın ekipman bir işe yaramadı.";
                        ItemSuccessful.texture = fail;
                        break;
                }
            }
        }
        else if (name == "GÖREV YAPILIYOR: SLOGAN AT")
        {
            if (itemwhicused != null)
            {
                StartCoroutine(findItem());
                switch (itemwhicused)
                {
                    case "HOH":
                        randomArkadas = Random.Range(10, 16);
                        randomAsker = Random.Range(4, 7);
                        textExplonation.text = "Halk Özel Harekat ve onların gücü sayesinde milletin 'Asker Kışlaya' haykırışları semalarda!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "YAYINGUCU":
                        randomArkadas = Random.Range(10, 16);
                        randomAsker = Random.Range(4, 7);
                        textExplonation.text = "Yayın gücü sayesinde, darbecilerin halka uyguladığı vahşeti anbean millete ulaştırdın. Artık daha fazla insan darbeye karşı durmak için sokaklarda!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "ARABAMEGAFONLU":
                        randomArkadas = Random.Range(8, 13);
                        randomAsker = Random.Range(1, 3);
                        textExplonation.text = "Arabaya megafon takmak dahiyane bir fikir! Artık, yalnızca megafon kullanarak sesini duyuramayacağın yerlere de ulaşabileceksin.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "MEGAFON":
                        randomArkadas = Random.Range(8, 13);
                        randomAsker = Random.Range(1, 3);
                        textExplonation.text = "Megafon sayesinde darbeye karşı vatandaşları örgütledin, ortamdaki maneviyatı üst düzeye çıkardın.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "BAYRAK":
                        randomArkadas = Random.Range(6, 8);
                        randomAsker = Random.Range(0, 1);
                        textExplonation.text = "Bayrak kullanarak hem kendinin hem diğer vatandaşların maneviyatını yükselttin, daha fazla insanı darbeye karşı sokağa çıkardın.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "TELEFON":
                        randomArkadas = Random.Range(6, 8);
                        randomAsker = Random.Range(0, 1);
                        textExplonation.text = "Slogan atarken telefon kullanarak sokakta neler yaşandığını sosyal medya aracılığıyla milyonlara ulaştırdın!";
                        ItemSuccessful.texture = successful;
                        break;
                    default:
                        randomArkadas = Random.Range(2, 4);
                        textExplonation.text = "Kullandığın ekipman bir işe yaramadı.";
                        ItemSuccessful.texture = fail;
                        break;
                }
            }
        }
        else if (name == "GÖREV YAPILIYOR: ASKERİ DURDUR")
        {
            if (itemwhicused != null)
            {
                StartCoroutine(findItem());
                switch (itemwhicused)
                {
                    case "PATLAYICIVARIL":
                        randomMecal = Random.Range(25, 36);
                        randomAsker = Random.Range(10, 16);
                        randomTank = Random.Range(1, 5);
                        textExplonation.text = "Varili patlattın ve darbecilerin yanısıra masumlara da zarar verdin. Unutma çatışmalarda büyük zarar görebilirsin!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "GAZBOMBASI":
                        randomMecal = Random.Range(0, 10);
                        randomAsker = Random.Range(10, 16);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Gaz bombası ile darbecileri etkisiz hale getirebilirsin. Böylece darbecileri onlarla çatışmadan da durdurabilirsin";
                        ItemSuccessful.texture = successful;
                        break;
                    case "SNIPERTUFEGI":
                        randomMecal = Random.Range(15, 20);
                        randomAsker = Random.Range(1, 4);
                        textExplonation.text = "Sniper tüfeği ile darbecileri durdurabilirsin ancak unutma sen vatandaşsın ve görevin çatışmak değil. Çünkü çatışma esnasında ölebilirsin!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "HOH":
                        randomMecal = Random.Range(15, 26);
                        randomAsker = Random.Range(6, 12);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Halk Özel Harekat sayesinde darbecileri durdurdun. Milletin gücü hainleri püskürttü!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "ARABAMEGAFONLU":
                        randomAsker = Random.Range(4, 8);
                        textExplonation.text = "Megafonlu araba ile masum askerleri darbe girişiminden haberdar ettin ve onları millet gücüne kattın!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "MAKINELITUFEK":
                        randomMecal = Random.Range(20, 25);
                        randomAsker = Random.Range(3, 7);
                        textExplonation.text = "Makineli tüfek ile darbecileri durdurabilirsin ancak unutma bu tehlikeli! Üstelik çatışma sırasında ölebilirsin!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "BICAK":
                        randomMecal = Random.Range(10, 15);
                        randomAsker = Random.Range(2, 5);
                        textExplonation.text = "Bıçak ile darbecileri durdurabilirsin ancak unutma masum askerler de var. Üstelik çatışma sırasında ölebilirsin!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "CELIKYELEK":
                        randomAsker = Random.Range(3, 5);
                        textExplonation.text = "Çelik yelek ile halkın üstüne ateş açan darbecilerden korunabilirsin. Yaralanmazsın ve böylece darbecileri etkisiz hale getirebilirsin";
                        ItemSuccessful.texture = successful;
                        break;
                    case "MEGAFON":
                        randomAsker = Random.Range(2, 6);
                        textExplonation.text = "Megafon kullanarak darbeden habersiz masum askerleri yayına çekmeyi başardın, onlara vatan sevgisini hatırlattın.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "BAYRAK":
                        randomAsker = Random.Range(1, 5);
                        textExplonation.text = "Bayrak kullanarak darbeden habersiz masum askerleri yayına çekmeyi başardın, onlara vatan sevgisini hatırlattın.";
                        ItemSuccessful.texture = successful;
                        break;
                    default:
                        randomMecal = Random.Range(5, 10);
                        randomAsker = Random.Range(0, 3);
                        textExplonation.text = "Kullandığın ekipman bir işe yaramadı.";
                        ItemSuccessful.texture = fail;
                        break;
                }
            }
        }
        else if (name == "GÖREV YAPILIYOR: TANKI DURDUR")
        {
            if (itemwhicused != null)
            {
                StartCoroutine(findItem());
                switch (itemwhicused)
                {
                    case "PATLAYICIVARIL":
                        randomMecal = Random.Range(25, 45);
                        randomAsker = Random.Range(5, 7);
                        randomTank = Random.Range(9, 16);
                        textExplonation.text = "Varili patlattın ve tankların yanısıra masumlara da zarar verdin. Unutma çatışmalarda büyük zarar görebilirsin!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "HOH":
                        randomMecal = Random.Range(10, 20);
                        randomAsker = Random.Range(3, 5);
                        randomTank = Random.Range(7, 9);
                        textExplonation.text = "Halk Özel Harekat (HÖH) olarak tankların etrafını kuşattın ve darbecileri teslim aldın!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "ARABAMEGAFONLU":
                        randomAsker = Random.Range(2, 4);
                        randomTank = Random.Range(1, 3);
                        textExplonation.text = "Megafonlu araba sayesinde hem tankın içindeki darbecileri vatana ihanet etmemeleri konusunda uyarabilirsin hem de tankın ilerlememesi için arabanı feda edebilirsin. ";
                        ItemSuccessful.texture = successful;
                        break;
                    case "ARABA":
                        randomAsker = Random.Range(1, 3);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Araba kullanarak tankın geçişini engelleyebilirsin. Vatan için bir araba feda etmek ancak teferruattır.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "VARILTAM":
                        randomAsker = Random.Range(1, 3);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Su dolu varili tankın ilerleyişini engellemek için kullandın ve başarılı oldun.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "KONTEYNER":
                        randomAsker = Random.Range(1, 3);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Konteyner kullanman sayesinde tankın ilerleyişini engelledin. Sakın pes etme tank kendine yeni bir yol bulacaktır.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "TABELA":
                        randomAsker = Random.Range(1, 3);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Tabelayı demir çubuk gibi kullanarak tankın paletlerine yerleştirdin ve tankı durdurdun. Şimdi darbecileri durdur!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "DEMIRCUBUK":
                        randomAsker = Random.Range(1, 3);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Demir çubukları tankın paletlerine yerleştirdin ve tankın ilerlemesini engelledin!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "ISLAKBEZ":
                        randomAsker = Random.Range(1, 3);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Islak bezi tankın egzozuna soktun ve böylece tankı durdurdun.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "SPREYBOYA":
                        randomAsker = Random.Range(1, 3);
                        randomTank = Random.Range(0, 2);
                        textExplonation.text = "Sprey boyayı tankın camına sıkarak darbecilerin yolu görmesine engel oldun ve tankı durdurdun.";
                        ItemSuccessful.texture = successful;
                        break;
                    case "SNIPERTUFEGI":
                        randomMecal = Random.Range(15, 25);
                        randomAsker = Random.Range(3, 6);
                        randomTank = Random.Range(0, 1);
                        textExplonation.text = "Sniper tüfeği ile darbecileri durdurabilirsin ancak unutma çatışma esnasında ölebilirsin!";
                        ItemSuccessful.texture = successful;
                        break;
                    case "MAKINELITUFEK":
                        randomMecal = Random.Range(10, 20);
                        randomAsker = Random.Range(1, 4);
                        randomTank = Random.Range(0, 1);
                        textExplonation.text = "Makineli tüfek kullanarak darbecileri durdurmuş olabilirsin ancak unutma karşı ateş açan askerler seni öldürebilir.";
                        ItemSuccessful.texture = successful;
                        break;
                    default:
                        randomMecal = Random.Range(10, 20);
                        textExplonation.text = "Kullandığın ekipman bir işe yaramadı.";
                        ItemSuccessful.texture = fail;
                        break;
                }
            }
        }
        else if (name == "GÖREV YAPILIYOR: YOLU ARAÇ TRAFİĞİNE KAPA")
        {
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() - 5);
            if (itemwhicused == "HILTITAM")
            {
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(findItem());
                }

                randomAsker = Random.Range(10, 20);
                randomTank = Random.Range(6, 12);
                textExplonation.text = "Hilti ile yolu kazıyaraktan tüm yolu trafiğe kapattın. Tanklar geçemiyor.";
            }
            else
            {
                StartCoroutine(findItem());
                randomAsker = Random.Range(5, 10);
                randomTank = Random.Range(3, 6);
                textExplonation.text = "Bu görevi, görevin özel itemı ile yapmış olsaydın çok daha fazla darbeciyi ve tankı etkisiz hale getirebilirdin.";
                ItemSuccessful.texture = fail;
            }
        }
        else if (name == "GÖREV YAPILIYOR: YAYINI ÖZGÜRLEŞTİR")
        {
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() - 10);
            if (itemwhicused == "HACKYETENEGI")
            {
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(findItem());
                }

                randomAsker = Random.Range(10, 20);
                randomTank = Random.Range(6, 12);
                textExplonation.text = "Kanalın yayın sistemini hackleyerek yayın akışını ele geçirdin";
                ItemSuccessful.texture = successful;
            }
            else
            {
                StartCoroutine(findItem());
                randomAsker = Random.Range(5, 10);
                randomTank = Random.Range(3, 6);
                textExplonation.text = "Bu görevi, görevin özel itemı ile yapmış olsaydın çok daha fazla asker ve tankı etkisiz hale getirebilirdin.";
                ItemSuccessful.texture = fail;
            }
        }
        else if (name == "GÖREV YAPILIYOR: KESKİN NİŞANCIYI ETKİSİZ HALE GETİR")
        {
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() - 15);
            if (itemwhicused == "SNIPERTUFEGI")
            {
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(findItem());
                }

                randomAsker = Random.Range(10, 20);
                randomTank = Random.Range(6, 12);
                textExplonation.text = "Keskin nişancı tüfeğiyle sniperı ve cuntacıların başındaki generali etkisiz hale getirdin. Cuntacılar birer birer teslim olmaya başladılar.";
                ItemSuccessful.texture = successful;
            }
            else
            {
                StartCoroutine(findItem());
                randomAsker = Random.Range(5, 10);
                randomTank = Random.Range(3, 6);
                textExplonation.text = "Bu görevi, görevin özel itemı ile yapmış olsaydın çok daha fazla asker ve tankı etkisiz hale getirebilirdin.";
                ItemSuccessful.texture = fail;
            }
        }
        else if (name == "GÖREV YAPILIYOR: KONTROL KULESİNİ ELE GEÇİR")
        {
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() - 20);
            if (itemwhicused == "GAZBOMBASI")
            {
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(findItem());
                }

                randomAsker = Random.Range(10, 20);
                randomTank = Random.Range(6, 12);
                textExplonation.text = "Göz yaşartıcı gaz ile içerideki cuntacıları ve liderlerini bayılttın. Kontrol kulesinin hakimiyeti sende!";
                ItemSuccessful.texture = successful;
            }
            else
            {
                StartCoroutine(findItem());
                randomAsker = Random.Range(5, 10);
                randomTank = Random.Range(3, 6);
                textExplonation.text = "Bu görevi, görevin özel itemı ile yapmış olsaydın çok daha fazla asker ve tankı etkisiz hale getirebilirdin.";
                ItemSuccessful.texture = fail;
            }
        }
        else if (name == "GÖREV YAPILIYOR: MASUM ASKERLERİ KURTAR")
        {
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() - 25);
            if (itemwhicused == "HOH")
            {
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(findItem());
                }

                randomAsker = Random.Range(10, 20);
                randomTank = Random.Range(6, 12);
                textExplonation.text = "HALK ÖZEL HAREKAT (HÖH) olarak cuntacıları inlerinde sıkıştırdın. Liderleri tutsak, emri altındaki masum askerleri kurtardın. Vatan sana minnettar!";
                ItemSuccessful.texture = successful;
            }
            else
            {
                StartCoroutine(findItem());
                randomAsker = Random.Range(5, 10);
                randomTank = Random.Range(3, 6);
                textExplonation.text = "Bu görevi, görevin özel itemı ile yapmış olsaydın çok daha fazla asker ve tankı etkisiz hale getirebilirdin.";
                ItemSuccessful.texture = fail;
            }
        }
        else
        {

        }

        //UsedItem
        if (ItemUsed.texture == noitem)
        {
            ItemUsed.gameObject.GetComponent<CanvasGroup>().alpha = 0.3f;
            textExplonation.text = "Hiçbir itemını kullanmadın";
        }
        else
        {
            ItemUsed.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        }

        //Kullanılan item ı azalt
        int amountusedItem = PlayerPrefs.GetInt(itemwhicused);
        PlayerPrefs.SetInt(itemwhicused, amountusedItem - 1);

        //Can
        if (randomMecal > 0)
        {
            PlayerPrefs.SetFloat("Mecal", mecal - randomMecal);
            Canmetre.rectTransform.localScale = new Vector3(randomMecal / 100.0f, Canmetre.rectTransform.localScale.y);
            lostCan.text = "-" + randomMecal;
        }

        //Medal
        randomMedal = Random.Range(0, 8);
        if (randomMedal == 1)
        {
            PlayerPrefs.SetInt("ItemDiamond", medal + 1);
            gainedMedal.text = 1.ToString();
            XPDiamond = 30;
        }
        else
        {
            XPDiamond = 0;
        }

        //Arkadas
        if (randomArkadas > 0)
        {
            XPFriend = randomArkadas * 10;
            gainedfriends.text = randomArkadas.ToString();

            int loc = PlayerPrefs.GetInt("Location");
            int amount = PlayerPrefs.GetInt("arkadas" + 1);
            PlayerPrefs.SetInt("arkadas" + loc, amount + randomArkadas);

            arkadasdegisimClone = Instantiate(ArkadasDegisim);
            arkadasdegisimClone.transform.SetParent(locationButtons[loc].transform, false);
            arkadasdegisimClone.gameObject.GetComponent<Text>().CrossFadeAlpha(0, 10.0f, false);
            arkadasdegisimClone.gameObject.GetComponent<Text>().text = "+" + randomArkadas.ToString();
            Destroy(arkadasdegisimClone, 15);

            if (randomArkadas >= 10)
            {
                Social.ReportProgress(Constants.achievement_halk_kahraman, 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
        }

        //Asker
        if (randomAsker > 0)
        {
            PlayerPrefs.SetInt("SOLDIER", asker + randomAsker);
            XPSoldier = randomAsker * 15;
            killedSoldier.text = randomAsker.ToString();
        }

        //Tank
        if (randomTank > 0)
        {
            PlayerPrefs.SetInt("TANK", tank + randomTank);
            XPTank = randomTank * 20;
            destroyedTanks.text = randomTank.ToString();

            if (randomTank >= 5)
            {
                Social.ReportProgress(Constants.achievement_tank_avcs, 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
        }

        //FoundItemandPoint
        int foundItemNumber = PlayerPrefs.GetInt("FoundedItem", 0);
        XPItem = foundItemNumber * 5;

        //TotalXP
        XPMission = XPFriend + XPSoldier + XPTank + XPItem + XPDiamond;
        totalXP.text = XPMission.ToString();
        PlayerPrefs.SetFloat("HalkGucu", getHalkGucu() + (XPMission / 20.0f));
        PlayerPrefs.SetInt("XP", XP + XPMission);

        //Görev Başarılı-Başarısız
        if (XPMission == 0)
        {
            Darbemetrecolor.gameObject.GetComponent<RawImage>().color = new Color(1.0F, 0, 0, 1.0F);
            textDarbe.color = new Color(1.0F, 0, 0, 1.0F);

            gorevBasarisiz.gameObject.SetActive(true);
        }
        else
        {
            CanvasDarbemetre.gameObject.SetActive(true);
            Darbemetrecolor.gameObject.GetComponent<RawImage>().color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
            textDarbe.color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
        }

        //Alphas
        if (randomMedal == 1)
        {
            sectionMedal.alpha = 1.0f;
        }
        else
        {
            sectionMedal.alpha = 0.25f;
        }

        if (randomMecal > 0)
        {
            sectionHealth.alpha = 1.0f;
        }
        else
        {
            sectionHealth.alpha = 0.25f;
        }

        if (randomArkadas > 0)
        {
            sectionFriends.alpha = 1.0f;
        }
        else
        {
            sectionFriends.alpha = 0.25f;
        }

        if (randomAsker > 0)
        {
            sectionSoldier.alpha = 1.0f;
        }
        else
        {
            sectionSoldier.alpha = 0.25f;
        }

        if (randomTank > 0)
        {
            sectionTank.alpha = 1.0f;
        }
        else
        {
            sectionTank.alpha = 0.25f;
        }

        getandSetMecal();
        getandSetSoldier();
        getandSetDiamond();
        getandSetTank();
        getandSetXP();
        closeCanvas(CanvasInMission);
    }

    //Çevreyi ara
    public IEnumerator findItem()
    {
        int itemNumber = PlayerPrefs.GetInt("FoundedItem", 0);
        int location = PlayerPrefs.GetInt("Location");

        Button item = null;
        int random;

        switch (location)
        {
            case 0:
                random = Random.Range(0, 12);
                item = homeitems[random];
                break;
            case 1:
                random = Random.Range(0, 16);
                item = citycenteritems[random];
                break;
            case 2:
                random = Random.Range(0, 11);
                item = pressbuildingitems[random];
                break;
            case 3:
                random = Random.Range(0, 10);
                item = bridgeitems[random];
                break;
            case 4:
                random = Random.Range(0, 14);
                item = airportitems[random];
                break;
            case 5:
                random = Random.Range(0, 12);
                item = militarybaseitems[random];
                break;
        }

        int random2 = Random.Range(0, 3);

        if (random2 > 0)
        {
            Button go = Instantiate(item) as Button;
            go.transform.SetParent(foundItemsPanel.transform, false);

            foreach (Transform t in go.transform)
            {
                if (t.name == "Text")
                {
                    t.gameObject.GetComponent<Text>().text = random2.ToString();
                }
            }

            int amount = PlayerPrefs.GetInt(item.name);
            PlayerPrefs.SetInt(item.name, amount + random2);

            bulundu[itemNumber].SetActive(true);
            itemNumber++;
            PlayerPrefs.SetInt("FoundedItem", itemNumber);
        }
        yield return new WaitForSeconds(0.1f);
    }

    public void resetMissionResult()
    {
        sectionMedal.alpha = 0.25f;
        sectionHealth.alpha = 0.25f;
        sectionFriends.alpha = 0.25f;
        sectionSoldier.alpha = 0.25f;
        sectionTank.alpha = 0.25f;

        gorevBasarisiz.gameObject.SetActive(false);

        randomMedal = 0;
        randomMecal = 0;
        randomArkadas = 0;
        randomAsker = 0;
        randomTank = 0;
        XPMission = 0;
        XPItem = 0;
        XPFriend = 0;
        XPSoldier = 0;
        XPTank = 0;
        PlayerPrefs.SetInt("FoundedItem", 0);
        PlayerPrefs.SetString("USEDITEM", null);

        gainedMedal.text = "";
        Canmetre.rectTransform.localScale = new Vector3(0, Canmetre.rectTransform.localScale.y);
        lostCan.text = "";
        gainedfriends.text = "";
        killedSoldier.text = "";
        destroyedTanks.text = "";
        totalXP.text = "";

        ItemUsed.texture = noitem;
        ItemUsed.gameObject.GetComponent<CanvasGroup>().alpha = 0.3f;
        ItemSuccessful.texture = fail;
        textExplonation.text = "";
        isSuitableforItemUsage = false;

        bulundu[0].SetActive(false);
        bulundu[1].SetActive(false);
        bulundu[2].SetActive(false);

        foreach (Transform t in foundItemsPanel.transform)
        {
            Destroy(t.gameObject);
        }
    }

    //CanvasDiamond
    public void earnDiamondviaAds()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() + 3);
                getandSetDiamond();
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
    }

    //CanvasAbout
    public void setAboutPage(int i)
    {
        if (i == 0)
        {
            sectionone.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            sectiontwo.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            sectionthree.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            panel1.gameObject.SetActive(true);
            panel2.gameObject.SetActive(false);

            titleofAbout.text = "Nasıl oynanır?";
            contentofAbout.text = "Darbe oyunu dört karakter ve altı mekândan oluşuyor. Karakter seçimi ile başlayan oyun sizi önce ev'e yönlendirir.\n\nSeçtiğiniz karakter ile ilk önce evde teçhizat aramalısınız. Teçhizatlar oyunun tamamında çok önemli bir yerde konumlanıyor. Eğer yeterli teçhizatınız yoksa darbeyi durduramazsın!\n\nEvden sonra Basın Binası, Köprü, Havaalanı, Askeri Üs, Şehir Merkezi mekânlarından birine gidip darbeyi durdurmak için gereken birtakım görevleri yapmalısınız. \n\nMekân seçerken gideceğiniz yerin yanında yazılı olan Kahraman (darbeye karşı savaşan vatansever) ve şehit (darbecilerle mücadele ederken yaşamını yitiren kahraman) sayılarına dikkat etmeniz ve bu yolda bir strateji belirlemeniz gerekir. \n\nOyun oynamayı devam etmeniz için iki tane bara dikkat etmelisiniz. Bunlardan biri darbemetre biri de can kutunuz. Canınız azaldıkça darbeyi durdurma ihtimaliniz düşer, darbemetre ilerledikçe canınız çok olsa bile darbe girişimi başarılı olur ve oyunu kaybedersiniz.\n\n<size=50><color=red>Görevler:</color></size>\n\n<size=40><color=#7F2E2DFF>Teçhizat ara:</color></size> Bulduğunuz teçhizatları mantıklı şekilde birleştirerek farklı materyaller elde edeceksiniz. Bu, darbeyi durdurmak için hayati önem taşıyor. Örneği su ve bezi birleştirirseniz ıslak bez elde edersiniz ve bu teçhizatınızı tankın egzozuna sokarak tankı durdurabiliriniz. Başka bir örnek, telefon ve bilgisayarı birleştirerek internetli bilgisayar elde edersiniz bu sayede daha fazla kahraman toplayabilirsiniz, bu toplama bir de kamera eklerseniz yayın gücü elde etmiş olursunuz. Bu örneklerin yolunda, sizin için hazırladığımız pek çok birleştirilebilir ögeyi oyunu oynarken kendiniz keşfedeceksiniz.\n\n<size=40><color=#7F2E2DFF>Kahraman topla:</color></size> Kahraman sayınız ne kadar çok olursa darbeyi önleme ihtimaliniz de o kadar artar. Kahraman toplama görevini yaparken telefon, megafon, halk özel harekat, internetli bilgisayar, megafonlu araba, bayrak ögelerini kullanabilirsiniz. Ayrıca sizce mantıklı olan ögeleri de birleştirmelisiniz.\n\n<size=40><color=#7F2E2DFF>Slogan at:</color></size> Slogan atarak hem meydanlarda toplanan insanları yüreklendirip hâlâ evde oturanların sokağa çıkmasını sağlayabilirsiniz hem de darbeden habersiz askerleri uyarabilirsiniz. Bu görevi icra ederken megafon, Halk özel harekat, yayın gücü, megafonlu araba, bayrak ve telefon kullanabilirsiniz. \n\n<size=40><color=#7F2E2DFF>Askeri durdur:</color></size> Askeri durdurmak en önemli görevlerden biri. Eğer doğru stratejiyi uygularsanız kan dökülmeden darbeyi önlemeniz mümkün olur. Bu görevde kullanılabilen bazı silahlı ögeler var. Bu şiddetli ögeleri kullanırken dikkat etmelisiniz; çünkü bu hem size hem de masum askerlere zarar verir ve darbenin başarılı olma ihtimalini artırır.  \n\n<size=40><color=#7F2E2DFF>Tankı durdur:</color></size> Tankı durdurarak darbeyi engelleme şansını oldukça yükseltirsin. Tankı durdurmak için kullanacaklarının arasında 15 Temmuz'dan aşina olduğun bazı ögeler var. Mesela tankın egzozuna ıslak bez, paletlerine demir çubuk sokmak gibi. Bu görevde de bazı şiddetli ögeler var, lütfen dikkat et ve masumlara zarar verme.\n\nHayal gücünü, vatan sevgini ve zekânı birleştir: Darbeyi durdur! \n\n<size=50><color=red>Özel görevler (Madalya kullanarak yapılır*)</color></size>\n\n- <size=40>Yolu araç trafiğine kapa (Şehir merkezi)</size>\n\n- <size=40>Yayını özgürleştir (Basın binası)</size>\n\n- <size=40>Keskin nişancıyı etkisiz hale getir (Köprü)</size>\n\n- <size=40>Kontrol kulesini ele geçir (Havaalanı)</size>\n\n- <size=40>Masum askerleri kurtar (Askeri üs)</size>\n\n*Oyuna başlarken üç madalyanız var. Reklam izleyerek, görevlerde başarılı olarak, sosyal medya paylaşımı yaparak veya para ile daha fazla madalya elde edebilirsiniz.";
        }
        else if (i == 1)
        {
            sectionone.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            sectionthree.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            sectiontwo.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            panel1.gameObject.SetActive(true);
            panel2.gameObject.SetActive(false);

            titleofAbout.text = "15 TEMMUZ";
            contentofAbout.text = "15 Temmuz 2016. Türkiye normal bir akşam yaşıyordu. Televizyonlar açık, hava sıcak, keyifler yerinde. Öyle normal bir akşam ki, belki çay eşliğinde çekirdek çitleniyor. Sonra seri ve sürekli uçak sesi geliyor etraftan. Allah Allah, bu saatte ne oluyor? Balkondan bakınca gürültüyü uçakların değil F16'ların çıkardığını anlaşılıyor. Haberler açılıyor heyecan ve kızgınlıkla, sosyal medyaya üşüşüyor herkes. Kimi darbe olacak diyor, kimi terör. Aradan fazla zaman geçmeden durum daha net görülüyor. Sokaklarda tanklar var. Kışlada olması gereken askerler, onların tankları, silahları şehrin sokaklarında. Eş dost aranıyor, “Ya hu, ne oluyor? Bu çağda darbe mi yapılır? Olamaz, darbe değildir.” Arkadaşlar, akrabalar, sosyal medyada söylentiler yayılıyor, derken Başbakan çıkıyor karşımıza bir haber kanalında: Bu bir darbe girişimi kalkışmasıdır. Başbakan halkı sokaklara davet ediyor, vatanı korumak için.\n\nSilah sesleri yükseliyor vatanın dört bir yanından.Vatandaşlar, nasıl evde dururlar? Darbe görmüş bir nesil, darbeyi çok iyi tanıyan, apolitik sanılan ama vatan savunmasında en ön safta olan insanlar sokağa çıkıyorlar.Mensubu oldukları vatanı savunmak için evlerinden çıkıyorlar.Kimi namaza duruyor Allah'tan güç istiyor, kimi konu komşuyu toplayarak maneviyatını güçlendiriyor. Çünkü oraya bir halk gerekiyor.\n\nAsker sokakta, halk sokakta, polis sokakta. Asker polisle, asker halkla karşı karşıya.Yaşandı bu sahneler, o yıllarda genç olanlar en iyi bilirler. O yıllarda yaşanmayan sahneler vardı ama bu kez. Asker halkı tarıyordu, halk evine gitmiyordu. Halk sokakları terk etmedi. Darbe girişimi, girişim olmaktan ileri gidemedi. Hak ve özgürlükleri korumak için yine şehit oldu vatanseverler.\nBu oyun 15 Temmuz Darbe Girişimi'ni engellemek için hayatını feda edenleri saygıyla anmak, anılarına dair yapılan işlerle ilgili çorbaya bir tuz katmak, halkın heyecanını canlı kılmak için oluşturuldu. Karanlık gecenin şafağında, patlayan o son bomba, devamında süren çatışmalar, gözaltılar ve tutuklamalar sonucunda darbenin bir girişim kalkışmasından ileri gidemediği için büyük memnuniyet duyuyor, şehitlerimizin nur içinde yatması için dua ediyoruz. Çünkü biz halkız, vatanını seven, vatanı için televizyon izlediği bir cuma akşamı rahatını bozup, yüreğini avcuna inancını aklına koyup sokağa çıkanların gerekirse kendini feda edenlerin yaşadığı bu topraklara sahip çıkanlarız. Sokaklarda olmanın gücünü, halkın gücünü tanıyanlardanız. Televizyonlar karardığında, polis harekat merkezleri bombalandığında hatta milletin meclisine sabotaj yapıldığında bile cesareti eksilmeyen, bir ölüp bin dirileceğine inanan ve kara leke oluşmadan engelleyebileceğini bilen o insanlarız. Atalarımızın yolundayız, ilkemiz, inkılabımız ve yasalarımızla, hâkimiyetin bizim olduğunu unutmayan, haklarımızın maliki olanlardanız.";
        }
        else
        {
            sectionone.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            sectiontwo.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            sectionthree.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            textVersion.text = Application.version;

            panel1.gameObject.SetActive(false);
            panel2.gameObject.SetActive(true);
        }
    }

    //CanvasGameOver
    public void gameOver()
    {
        if (Advertisement.IsReady("video"))
        {
            Advertisement.Show("video");
        }
        CancelInvoke();
        StopAllCoroutines();
        openCanvas(CanvasGameOver);
        gameoverDiamond.text = getandSetDiamond().ToString();
        gameoverSoldier.text = getandSetSoldier().ToString();
        gameoverTank.text = getandSetTank().ToString();
        gameoverFriend.text = PlayerPrefs.GetInt("TotalArkadas").ToString();
        gameoverPoint.text = getandSetXP().ToString();
    }

    public void restartTheGame()
    {
        Social.ReportScore(getandSetXP(), Constants.leaderboard_en_iyiler, (bool success) =>
        {
            // handle success or failure
        });

        string kullaniciadi = PlayerPrefs.GetString("Name");
        int diamond = getandSetDiamond();
        string sound = PlayerPrefs.GetString("Sound");
        //Delete everyting
        PlayerPrefs.DeleteAll();
        //And save some of them again
        PlayerPrefs.SetString("Name", kullaniciadi);
        PlayerPrefs.SetInt("ItemDiamond", diamond);
        PlayerPrefs.SetString("Sound", sound);
        SceneManager.LoadSceneAsync("Scene0", LoadSceneMode.Single);
    }

    //Ateş açılma
    public IEnumerator AtesAcildi()
    {
        int random = Random.Range(15, 60);
        yield return new WaitForSeconds(random);

        //Start the fire
        int randomLoc = Random.Range(1, 5);

        CanvasNewsFeed2.gameObject.SetActive(true);
        StartCoroutine("blinkingAtesAcildi");

        switch (randomLoc)
        {
            case 1:
                atesAcildiText.text = "<color=red>ŞEHİR MERKEZİNDE ATEŞ AÇILDI</color>";
                break;
            case 2:
                atesAcildiText.text = "<color=red>BASIN BİNASINDA ATEŞ AÇILDI</color>";
                break;
            case 3:
                atesAcildiText.text = "<color=red>KÖPRÜDE ATEŞ AÇILDI</color>";
                break;
            case 4:
                atesAcildiText.text = "<color=red>HAVAALANINDA ATEŞ AÇILDI</color>";
                break;
            case 5:
                atesAcildiText.text = "<color=red>ASKERİ ÜSTE ATEŞ AÇILDI</color>";
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(3.0f);
        for (int i = 0; i < 5; i++)
        {
            if (randomLoc == PlayerPrefs.GetInt("Location"))
            {
                mermiIsabet();
            }
            yield return new WaitForSeconds(3.0f);
        }

        CanvasNewsFeed2.gameObject.SetActive(false);
        StopCoroutine("blinkingAtesAcildi");
        StartCoroutine("AtesAcildi");
    }

    public void mermiIsabet()
    {
        GameObject mermi = Instantiate(atesAcildi);
        float hp = getandSetMecal() - 5.0f;
        PlayerPrefs.SetFloat("Mecal", hp);
        mermi.gameObject.GetComponent<RawImage>().CrossFadeAlpha(0, 3.0f, false);
        Destroy(mermi, 3);
    }

    public IEnumerator blinkingAtesAcildi()
    {
        atesAcildiText.CrossFadeAlpha(0.0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        atesAcildiText.CrossFadeAlpha(1.0f, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("blinkingAtesAcildi");
    }

    //Some functions
    public int getgameStatus()
    {
        int gameStatus = PlayerPrefs.GetInt("Status", 0);
        if (gameStatus == 0)
        {
            //Game still not start
        }
        else if (gameStatus == 1)
        {
            //Game start or continuing
        }
        else if (gameStatus == 2)
        {
            //You lose
            CanvasGameOver.gameObject.GetComponent<RawImage>().texture = darbebasarili;
            GameOverorWin.text = "darbeyi durduramadın.";
            gameOver();
        }
        else if (gameStatus == 3)
        {
            //You win
            CanvasGameOver.gameObject.GetComponent<RawImage>().texture = darbebasarisiz;
            GameOverorWin.text = "darbeyi durdurdun.";
            gameOver();
            Social.ReportProgress(Constants.achievement_demokrasi_kahraman, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        else if (gameStatus == 4)
        {
            //You died
            CanvasGameOver.gameObject.GetComponent<RawImage>().texture = oldu;
            GameOverorWin.text = "demokrasi şehidi oldun.";
            gameOver();
            Social.ReportProgress(Constants.achievement_demokrasi_ehidi, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }
        else
        {
            //Error
            restartTheGame();
        }

        return gameStatus;
    }

    public void setProfileImageandName()
    {
        string pImage = PlayerPrefs.GetString("Character");
        switch (pImage)
        {
            case "Male1":
                Character.texture = characters[0];
                break;
            case "Male2":
                Character.texture = characters[1];
                break;
            case "Female1":
                Character.texture = characters[2];
                break;
            case "Female2":
                Character.texture = characters[3];
                break;
            default:
                break;
        }
    }

    public float getHalkGucu()
    {
        float halkgucu = PlayerPrefs.GetFloat("HalkGucu");
        return halkgucu;
    }

    public int getandSetXP()
    {
        int XP = PlayerPrefs.GetInt("XP", 0);
        textXP.text = XP.ToString();

        if (XP > 0)
        {
            Social.ReportProgress(Constants.achievement_halk_gcne_katldn, 100.0f, (bool success) =>
            {
                // handle success or failure
            });
        }

        return XP;
    }

    public int getandSetSoldier()
    {
        int soldier = PlayerPrefs.GetInt("SOLDIER", 0);
        if (soldier > 0)
        {
            Soldier.gameObject.GetComponent<Image>().color = new Color(1F, 1F, 1F, 1.0F);
        }
        textSoldier.text = soldier.ToString();
        return soldier;
    }

    public int getandSetTank()
    {
        int tank = PlayerPrefs.GetInt("TANK", 0);
        if (tank > 0)
        {
            Tank.gameObject.GetComponent<Image>().color = new Color(1F, 1F, 1F, 1.0F);
        }
        textTank.text = tank.ToString();
        return tank;
    }

    public int getandSetDiamond()
    {
        int diamond = PlayerPrefs.GetInt("ItemDiamond", 3);
        if (diamond <= 0)
        {
            diamond = 0;
        }

        textDiamond.text = diamond.ToString();
        textDiamond2.text = diamond.ToString();
        textDiamond3.text = diamond.ToString();

        return diamond;
    }

    //Transition
    public void openCanvas(Canvas canvas)
    {
        StartCoroutine(changeCanvas(null, canvas));
    }

    public void closeCanvas(Canvas canvas)
    {
        StartCoroutine(changeCanvas(canvas, null));
    }

    public IEnumerator changeCanvas(Canvas closing, Canvas opening)
    {
        //FadeIn-Out
        if (!isLocationChanging)
        {
            Instantiate(screenTransition);
            yield return new WaitForSeconds(0.375f);
        }

        //Close
        if (closing != null && !isLocationChanging)
        {
            if (closing == CanvasOpening)
            {
                PlayerPrefs.SetInt("Status", 1);
                getgameStatus();

                Darbemetrecolor.gameObject.GetComponent<RawImage>().color = Color.black;
                textDarbe.color = Color.black;
            }
            else if (closing == CanvasMain)
            {
                CanvasDarbemetre.gameObject.SetActive(false);
                CanvasNewsFeed.gameObject.SetActive(false);
                CanvasNewsFeed2.gameObject.SetActive(false);
                checkGameStatus();
                getandSetDiamond();
                CancelInvoke();
                StopAllCoroutines();
            }
            else if (closing == CanvasInventory)
            {
                if (CanvasMissionPrompt.isActiveAndEnabled)
                {
                    updateInventoryforMission();
                }
                else if (CanvasMissionResult.isActiveAndEnabled)
                {
                    CanvasBottomBar.gameObject.SetActive(false);
                    CanvasNewsFeed.gameObject.SetActive(false);
                    CanvasDarbemetre.gameObject.SetActive(true);
                    Darbemetrecolor.gameObject.GetComponent<RawImage>().color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
                    textDarbe.color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
                }
                else if (CanvasLocation.isActiveAndEnabled)
                {
                    //Do nothing but this is necessary
                }
                else
                {
                    CanvasBottomBar.gameObject.SetActive(false);
                    CanvasDarbemetre.gameObject.SetActive(true);
                    Darbemetrecolor.gameObject.GetComponent<RawImage>().color = Color.black;
                    textDarbe.color = Color.black;
                }
                Invoke("clearInventory", 0.375f);
                CanvasMain.gameObject.GetComponent<AudioSource>().volume = 1.0f;
            }
            if (closing == CanvasLocation)
            {
                if (isLocationChanging)
                {
                    closing = null;
                }
                else
                {
                    CanvasDarbemetre.gameObject.SetActive(true);
                    CanvasBottomBar.gameObject.SetActive(false);
                }
            }
            else if (closing == CanvasMissionResult)
            {
                CanvasDarbemetre.gameObject.SetActive(false);
                CanvasBottomBar.gameObject.SetActive(true);
                CanvasNewsFeed.gameObject.SetActive(true);
                CanvasNewsFeed2.gameObject.SetActive(true);

                Darbemetrecolor.gameObject.GetComponent<RawImage>().color = Color.black;
                textDarbe.color = Color.black;

                Invoke("resetMissionResult", 1);
            }
            else if (closing == CanvasDiamond)
            {
                if (CanvasLocation.isActiveAndEnabled)
                {
                    CanvasDarbemetre.gameObject.SetActive(false);
                    CanvasBottomBar.gameObject.SetActive(true);
                    CanvasNewsFeed.gameObject.SetActive(true);
                    CanvasNewsFeed2.gameObject.SetActive(true);

                    //Arkadaş ve şehit sayısı düzenli artış
                    InvokeRepeating("ArkadasandSehit", 5, 5F);

                    //Halkgücünü düzenli azalt
                    InvokeRepeating("updateMeters", 5, 1F);

                    //NewsFeed
                    StartCoroutine("newsFeed");

                    //AtesAcildi
                    StartCoroutine("AtesAcildi");

                    //Ads
                    Invoke("showAd", 10);

                    //Sound
                    CanvasMain.gameObject.GetComponent<AudioSource>().volume = 1.0f;

                    darbeMetre();
                    getandSetMecal();
                }
                else if (Canvas1.isActiveAndEnabled)
                {
                    //Do nothing
                }
                else
                {
                    CanvasDarbemetre.gameObject.SetActive(true);
                    CanvasNewsFeed.gameObject.SetActive(true);
                    CanvasNewsFeed2.gameObject.SetActive(true);

                    //Arkadaş ve şehit sayısı düzenli artış
                    InvokeRepeating("ArkadasandSehit", 5, 5F);

                    //Halkgücünü düzenli azalt
                    InvokeRepeating("updateMeters", 5, 1F);

                    //NewsFeed
                    StartCoroutine("newsFeed");

                    //AtesAcildi
                    StartCoroutine("AtesAcildi");

                    //Sound
                    CanvasMain.gameObject.GetComponent<AudioSource>().volume = 1.0f;

                    darbeMetre();
                    getandSetMecal();
                }
            }
            else
            {

            }
            closing.gameObject.SetActive(false);
        }

        //Open
        if (opening != null && !isLocationChanging)
        {
            //GoogleAnalytics
            if ((opening != CanvasDarbemetre && opening != CanvasNewsFeed) && (opening != CanvasBottomBar && opening != CanvasNewsFeed2))
            {
                googleAnalytics.LogScreen(opening.name);
            }
            //Canvas opening settings
            if (opening == CanvasOpening)
            {
                Darbemetrecolor.gameObject.GetComponent<RawImage>().color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
                textDarbe.color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
                Invoke("showAd", 10.0f);
            }
            else if (opening == CanvasMain)
            {
                CanvasNewsFeed.gameObject.SetActive(true);
                CanvasDarbemetre.gameObject.SetActive(true);
            }
            else if (opening == CanvasInventory)
            {
                CanvasBottomBar.gameObject.SetActive(true);
                CanvasNewsFeed.gameObject.SetActive(true);
                CanvasDarbemetre.gameObject.SetActive(false);
                updateInventory();
                if (CanvasMissionPrompt.isActiveAndEnabled)
                {
                    StartCoroutine(resetMissionPrompt());
                }
                CanvasMain.gameObject.GetComponent<AudioSource>().volume = 0.25f;
            }
            else if (opening == CanvasLocation)
            {
                CanvasDarbemetre.gameObject.SetActive(false);
                CanvasBottomBar.gameObject.SetActive(true);
                CanvasNewsFeed.gameObject.SetActive(true);
            }
            else if (opening == CanvasInMission)
            {
                if (mission.FullScreenMission == null)
                {
                    horizontalMission.texture = mission.MissionPrompt;
                    panelNormal.gameObject.SetActive(true);
                    CanvasDarbemetre.gameObject.SetActive(true);
                    Darbemetrecolor.gameObject.GetComponent<RawImage>().color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
                    textDarbe.color = new Color(0.498F, 0.180F, 0.177F, 1.0F);
                }
                else
                {
                    fullscrenMission.texture = mission.FullScreenMission;
                    panelFullScreen.gameObject.SetActive(true);
                    CanvasDarbemetre.gameObject.SetActive(true);
                    Darbemetrecolor.gameObject.GetComponent<RawImage>().color = Color.black;
                    textDarbe.color = Color.black;
                }

                CanvasBottomBar.gameObject.SetActive(false);
                CanvasNewsFeed.gameObject.SetActive(false);
                CanvasNewsFeed2.gameObject.SetActive(false);
            }
            else if (opening == CanvasMissionResult)
            {
                panelNormal.gameObject.SetActive(false);
                panelFullScreen.gameObject.SetActive(false);
            }
            else if (opening == CanvasDiamond)
            {
                CanvasDarbemetre.gameObject.SetActive(false);
                CanvasNewsFeed.gameObject.SetActive(false);
                CanvasNewsFeed2.gameObject.SetActive(false);
                CanvasBottomBar.gameObject.SetActive(false);
                CancelInvoke();
                StopAllCoroutines();
                CanvasMain.gameObject.GetComponent<AudioSource>().volume = 0.0f;
            }
            else if (opening == CanvasGameOver)
            {
                CanvasMain.gameObject.SetActive(false);
                CanvasInventory.gameObject.SetActive(false);
                CanvasLocation.gameObject.SetActive(false);
                CanvasMissions.gameObject.SetActive(false);
                CanvasMissionPrompt.gameObject.SetActive(false);
                CanvasInMission.gameObject.SetActive(false);
                CanvasMissionResult.gameObject.SetActive(false);
                CanvasDarbemetre.gameObject.SetActive(false);
                CanvasBottomBar.gameObject.SetActive(false);
                CanvasNewsFeed.gameObject.SetActive(false);
                CanvasNewsFeed2.gameObject.SetActive(false);
            }
            else
            {

            }
            opening.gameObject.SetActive(true);
        }
    }

    //Ads
    private void loadAd()
    {
        string adUnitId = "ca-app-pub-1576175228836763/6176471338";
        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("0A83AF9337EAE655A7B29C5B61372D84").Build();
        interstitial.LoadAd(request);
    }

    public void prepareAd()
    {
        int random0 = Random.Range(60, 120);
        Invoke("showAd", (random0));
        Debug.logger.Log(random0);
    }

    public void showAd()
    {
        if (!checkPremium() && interstitial.IsLoaded())
        {
            interstitial.Show();
            CancelInvoke("showAd");
            prepareAd();
            loadAd();
        }
    }

    //In-App Purchase
    public void InitializeInApp()
    {
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Premium
        builder.AddProduct("premium", ProductType.NonConsumable, new IDs
             {
                 {"premium", GooglePlay.Name},
             });

        builder.AddProduct("diamond25", ProductType.Consumable, new IDs
             {
                 {"diamond25", GooglePlay.Name},
             });

        builder.AddProduct("diamond50", ProductType.Consumable, new IDs
             {
                 {"diamond50", GooglePlay.Name},
             });

        builder.AddProduct("diamond100", ProductType.Consumable, new IDs
             {
                 {"diamond100", GooglePlay.Name},
             });

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null;
    }

    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                //Daha önceden satın almış zaten
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            InitializeInApp();
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //Error
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        if (string.Equals(args.purchasedProduct.definition.id, "premium", System.StringComparison.Ordinal))
        {
            Debug.Log(string.Format("Premium alındı", args.purchasedProduct.definition.id));
            PlayerPrefs.SetString("Premium", "TRUE");
        }
        else if (string.Equals(args.purchasedProduct.definition.id, "diamond25", System.StringComparison.Ordinal))
        {
            Debug.Log(string.Format("25 Madalya alındı", args.purchasedProduct.definition.id));
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() + 25);
        }
        else if (string.Equals(args.purchasedProduct.definition.id, "diamond50", System.StringComparison.Ordinal))
        {
            Debug.Log(string.Format("50 Madalya alındı", args.purchasedProduct.definition.id));
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() + 50);
        }
        else if (string.Equals(args.purchasedProduct.definition.id, "diamond100", System.StringComparison.Ordinal))
        {
            Debug.Log(string.Format("100 Madalya alındı", args.purchasedProduct.definition.id));
            PlayerPrefs.SetInt("ItemDiamond", getandSetDiamond() + 100);
        }
        else
        {
            //Error
        }
        getandSetDiamond();
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //Error
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    //OpenLink
    public void openLink(string uri)
    {
        Application.OpenURL(uri);
    }
}

