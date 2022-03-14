using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automata_GameObjects : MonoBehaviour {
    public static int tam = 8, numOfPrefs;
    public static bool click;
    public static int numberOfInfections = 0;
    public GameObject body;
    public GameObject CelulaBase;
    public GameObject[] prefabs;

    public GameObject[,,] automata = new GameObject[tam, tam, tam];
    private readonly int period = 600;
    private bool isFullGraphicsScene;
    private float nextActionTime = 0.0f;

    // Start is called before the first frame update
    void Start() {
        nextActionTime = Mathf.FloorToInt(Time.time) + period;
        numOfPrefs = prefabs.Length;
        numberOfInfections = 0;
        /// <resume> Primera vez que se abre el programa </resume>
        if (SystemManager.savedData.firstTime) {
            //doTheEvolutionRandom();
            initialCondition();

            // Sistema de guardado
            SystemManager.savedData.firstTime = false;
            SaveSystem.saverData();
        }
        isFullGraphicsScene = !UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("LowGraphicsScene");
        Vector3 position = Vector3.zero;
        for (int i = 0; i < tam; i++) {
            for (int j = 0; j < tam; j++) {
                for (int k = 0; k < tam; k++) {
                    int typ = SystemManager.savedData.automataSaved[i, j, k];
                    if (typ != -1) {
                        automata[i, j, k] = Instantiate(CelulaBase, transform.position, Quaternion.identity, transform);
                        automata[i, j, k].GetComponent<Celula_GameObject>().CelulaGenerator(
                            prefabs[typ]
                            , new Vector3(position.x + i - (tam / 2)
                                , position.y + j - (tam / 2)
                                , position.z + k - (tam / 2))
                            , gameObject
                            , body
                            , typ
                            , isFullGraphicsScene
                        );
                        numberOfInfections++;
                    } else {
                        automata[i, j, k] = Instantiate(CelulaBase, transform.position, Quaternion.identity, transform);
                        automata[i, j, k].GetComponent<Celula_GameObject>().CelulaEmptyGenerator(); ;
                    }
                }
            }
        }
        click = true;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            doTheEvolutionBaby();
            click = true;
            SaveSystem.saverData();
        }
    }

    void initialCondition() {
        SystemManager.savedData.gen = 0;
        int sum = 0, clientNameLength = SystemManager.savedData.clientName.Length;
        for (int i = 0; i < clientNameLength; i++) {
            sum += SystemManager.savedData.clientName[i];
        }
        sum = sum % numOfPrefs;

        for (int i = 0; i < tam; i++) {
            for (int j = 0; j < tam; j++) {
                for (int k = 0; k < tam; k++) {
                    SystemManager.savedData.automataSaved[i, j, k] = -1;
                }
            }
        }

        int from = (tam / 2) - 1, to = (tam / 2) + 1;
        /// Recorre el sub cubo para crear una célula con virus o no, según el nombre
        for (int i = from; i <= to; i++) {
            for (int j = from; j <= to; j++) {
                for (int k = from; k <= to; k++) {
                    int born = SystemManager.savedData.clientName[(i + j + k) % clientNameLength] % 2;
                    if (born == 0) {
                        SystemManager.savedData.automataSaved[i, j, k] = sum;
                    }
                }
            }
        }
    }

    void doTheEvolutionBaby() {
        int spreadForce;
        for (int i = 0; i < tam; i++) {
            for (int j = 0; j < tam; j++) {
                for (int k = 0; k < tam; k++) {

                    if (automata[i, j, k].GetComponent<Celula_GameObject>().type != -1) {
                        spreadForce = checkNeightbord(i, j, k, automata[i, j, k].GetComponent<Celula_GameObject>().type);
                        if (spreadForce > 5) { // tiene el poder de contagiar
                            spred(i, j, k, automata[i, j, k].GetComponent<Celula_GameObject>().type, spreadForce);
                        }
                        int probToDie = Random.Range(0, 1000);
                        if ((!automata[i, j, k].GetComponent<Celula_GameObject>().cellObject.GetComponent<CelulaBehaviour>().live) ||
                            ((numberOfInfections > 12) && (probToDie > 900 - spreadForce))) {
                            automata[i, j, k].GetComponent<Celula_GameObject>().CelulaEmptyGenerator();
                            SystemManager.savedData.automataSaved[i, j, k] = -1;
                            numberOfInfections--;
                        }
                    }

                }
            }
        }
        //System.GC.Collect();
        //System.GC.WaitForPendingFinalizers();
        SystemManager.savedData.gen++;
    }

    int checkNeightbord(int a, int b, int c, int type) {
        int sum = 0;
        for (int i = a - 1; i <= a + 1; i++) {
            for (int j = b - 1; j <= b + 1; j++) {
                for (int k = c - 1; k <= c + 1; k++) {
                    if ((i >= 0) && (i < tam) && (j >= 0) && (j < tam) && (k >= 0) && (k < tam)) {
                        if (!((i == a) && (j == b) && (k == c))) {
                            if (automata[i, j, k].GetComponent<Celula_GameObject>().type != -1) {
                                sum++;
                            }
                        }
                    }
                }
            }
        }
        return sum;
    }

    void spred(int a, int b, int c, int type, int spreadForce) {
        int prob = 100;
        for (int i = a - 1; i <= a + 1; i++) {
            for (int j = b - 1; j <= b + 1; j++) {
                for (int k = c - 1; k <= c + 1; k++) {
                    if ((i >= 0) && (i < tam) && (j >= 0) && (j < tam) && (k >= 0) && (k < tam)) {
                        if (!((i == a) && (j == b) && (k == c))) {
                            if (automata[i, j, k].GetComponent<Celula_GameObject>().type == -1) {
                                int probToGetInfected = Random.Range(0, prob);
                                spreadForce = Mathf.Clamp(spreadForce, 0, 20);
                                if (probToGetInfected > prob - (spreadForce)) {
                                    Vector3 position = Vector3.zero;
                                    type = doesMute(type);
                                    automata[i, j, k].GetComponent<Celula_GameObject>().CelulaGenerator(
                                        prefabs[type]
                                        , new Vector3(position.x + i - (tam / 2)
                                            , position.y + j - (tam / 2)
                                            , position.z + k - (tam / 2))
                                        , gameObject
                                        , body
                                        , type
                                        , isFullGraphicsScene
                                    );
                                    numberOfInfections++;
                                    SystemManager.savedData.automataSaved[i, j, k] = type;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    int doesMute(int originalType) {
        int type = originalType;
        int rnd;
        for (int i = 0; i < 20; i++) {
            rnd = Random.Range(0, 1000);
            if (rnd > 990) {
                type = (type + 1) % numOfPrefs;
                break;
            }
        }
        return type;
    }
}
