using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automata : MonoBehaviour {
    public static int tam = 8, numOfPrefs;
    public static bool click;
    public static int numberOfInfections = 0;
    public GameObject body;
    public GameObject[] prefabs;

    public Celula[,,] automata = new Celula[tam, tam, tam];

    int period = 9;
    private float nextActionTime = 0.0f;
    //Celula[,,] buffer = new Celula[tam, tam, tam];

    // Start is called before the first frame update
    void Start() {
        click = false;
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
        } else {

        }
        Vector3 position = Vector3.zero;
        for (int i = 0; i < tam; i++) {
            for (int j = 0; j < tam; j++) {
                for (int k = 0; k < tam; k++) {
                    int typ = SystemManager.savedData.automataSaved[i, j, k];
                    if (typ != -1) {
                        automata[i, j, k] = null;
                        //System.GC.Collect();
                        //System.GC.WaitForPendingFinalizers();
                        automata[i, j, k] = new Celula(
                            prefabs[typ]
                            , new Vector3(position.x + i - (tam / 2)
                                , position.y + j - (tam / 2)
                                , position.z + k - (tam / 2))
                            , gameObject
                            , body
                            , typ
                        );
                        numberOfInfections++;
                    } else {
                        automata[i, j, k] = null;
                        //System.GC.Collect();
                        //System.GC.WaitForPendingFinalizers();
                        automata[i, j, k] = new Celula();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > nextActionTime) {
            //generation++;
            nextActionTime += period;
            // Tiene que cambiar para que se reemplaze el mesh a uno vacío cuando muere
            //doTheEvolutionRandom();
            doTheEvolutionBaby();
            Debug.Log("Evol");
            click = true;
            SaveSystem.saverData();
        }
    }

    void initialCondition() {
        //Debug.Log("inicial");
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
        //Vector3 position = Vector3.zero;
        for (int i = 0; i < tam; i++) {
            for (int j = 0; j < tam; j++) {
                for (int k = 0; k < tam; k++) {
                    if (automata[i, j, k].type != -1) {
                        spreadForce = checkNeightbord(i, j, k, automata[i, j, k].type);
                        if (spreadForce > 5) { // tiene el poder de contagiar
                            spred(i, j, k, automata[i, j, k].type, spreadForce);
                        }
                        int probToDie = Random.Range(0, 1000);
                        if ((!automata[i, j, k].cellObject.GetComponent<CelulaBehaviour>().live) ||
                            ((numberOfInfections > 10) && (probToDie > 800 - spreadForce))) {
                            Destroy(automata[i, j, k].cellObject);
                            automata[i, j, k] = null;
                            automata[i, j, k] = new Celula();
                            SystemManager.savedData.automataSaved[i, j, k] = -1;
                            numberOfInfections--;
                            //Debug.Log("Die");
                        }
                    }
                }
            }
        }
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        SystemManager.savedData.gen++;
    }

    int checkNeightbord(int a, int b, int c, int type) {
        int sum = 0;
        for (int i = a - 1; i <= a + 1; i++) {
            for (int j = b - 1; j <= b + 1; j++) {
                for (int k = c - 1; k <= c + 1; k++) {
                    if ((i >= 0) && (i < tam) && (j >= 0) && (j < tam) && (k >= 0) && (k < tam)) {
                        if (!((i == a) && (j == b) && (k == c))) {
                            if (automata[i, j, k].type != -1) {
                                sum++;
                            }
                        }
                    }
                }
            }
        }
        //sum += (a - 1 >= 0) && (automata[a - 1, b, c].vivo) && (automata[a - 1, b, c].type == type) ? 1 : 0;
        //sum += (a + 1 < tam) && (automata[a + 1, b, c].vivo) && (automata[a + 1, b, c].type == type) ? 1 : 0;

        //sum += (b - 1 >= 0) && automata[a, b - 1, c].vivo ? 1 : 0;
        //sum += (b + 1 < tam) && automata[a, b + 1, c].vivo ? 1 : 0;

        //sum += (c - 1 >= 0) && automata[a, b, c - 1].vivo ? 1 : 0;
        //sum += (c + 1 < tam) && automata[a, b, c + 1].vivo ? 1 : 0;

        //Debug.Log(automata[a, b, c].ToString() + " " + sum);
        return sum;
    }

    void spred(int a, int b, int c, int type, int spreadForce) {
        int prob = 140;
        for (int i = a - 1; i <= a + 1; i++) {
            for (int j = b - 1; j <= b + 1; j++) {
                for (int k = c - 1; k <= c + 1; k++) {
                    if ((i >= 0) && (i < tam) && (j >= 0) && (j < tam) && (k >= 0) && (k < tam)) {
                        if (!((i == a) && (j == b) && (k == c))) {
                            if (automata[i, j, k].type == -1) {
                                int probToGetInfected = Random.Range(0, prob);
                                if (probToGetInfected > prob - (spreadForce)) {
                                    Vector3 position = Vector3.zero;
                                    type = doesMute(type);
                                    automata[i, j, k] = null;
                                    automata[i, j, k] = new Celula(
                                        prefabs[type]
                                        , new Vector3(position.x + i - (tam / 2)
                                            , position.y + j - (tam / 2)
                                            , position.z + k - (tam / 2))
                                        , gameObject
                                        , body
                                        , type
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
        //System.GC.Collect();
        //System.GC.WaitForPendingFinalizers();
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

    //void copyToBuffer() {
    //    for (int i = 0; i < tam; i++) {
    //        for (int j = 0; j < tam; j++) {
    //            for (int k = 0; k < tam; k++) {
    //                buffer[i, j, k] = automata[i, j, k];
    //            }
    //        }
    //    }
    //}

    //void doTheEvolutionRandom() {
    //    float neightbord;
    //    for (int i = 0; i < tam; i++) {
    //        for (int j = 0; j < tam; j++) {
    //            for (int k = 0; k < tam; k++) {
    //                neightbord = Random.Range(0f, 6f);
    //                if (neightbord > 4f) {
    //                    automata[i, j, k].vivo = false;
    //                    automata[i, j, k].viveViveMuere();
    //                } else {
    //                    automata[i, j, k].vivo = true;
    //                    automata[i, j, k].viveViveMuere();
    //                }
    //            }
    //        }
    //    }
    //}
}
