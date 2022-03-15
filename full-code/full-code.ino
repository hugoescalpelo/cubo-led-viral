/*
 * Autores:
 * Anni Garza: Arte y concepto
 * Liliana Dominguez: Biología molecular
 * Hugo Vagas: Circuito electrónico
 * Yunuen Vladimir: Desarrollo de pieza digital y algoritmos
 * 
 * Este código corresponde a la pieza de arte "Viral", en la cual se expresa en un cubo led
 * la mutación de una hibridación entre el ADN de Anni y la proteina Spike del SARS-COV-2 COVID19
 * 
 * Este código es para el micro controlador nodeMCU ESP8266 con programador CP2102. 
 * El código se carga a través de la IDE de Arduino, la cual requiere las siguientes modificaciones:
 * 
 * File>Prefrences>Boards Manager>Add link
 * http://arduino.esp8266.com/stable/package_esp8266com_index.json
 * 
 * Tools>Boards>Boards Manager>ESP8266 
 * 
 * Bilbiotecas: Sketch>Include Library>Manage Libraries
 * DHT Sensor Library by Adafruit
 * FastLed by Daniel Garcia
 * 
 * Este programa alimenta un cubo de 8x8x8 leds neopixel tipo WS2812b
 * 
 * Los leds se dividen en 8 paneles de 8x8 leds en serie conectados en los pines D0, D1,
 * D2, D3, D4, D5, D6, D7.
 * 
 * Se usa un sensor DHT11 conectado al pin D8.
 * 
 * El algoritmo generado por Yunuen Vladimir debe detonar el cambio generacional cada que el
 * sensor de temperatura y humedad relativa DHT11 detecta un cambio significativo.
 * 
 * Para ese programa se ha usado una secuencia de comparaciones de tiempo no bloqueantes 
 * tambien conocidas como interrupciones por software basadas en tiempo. El objetivo es realizar
 * una lectura del sensor cada 1000 ms y alimentar un arreglo de anillo de longitud 16 con el fin
 * de notar cambios pronunciados e intencionales detonados por usuarios.
 * 
 * 
 */

//Biblioteca del sensor
#include "DHT.h"

//Constantes
#define DHTPIN D8//Pin donde se lee el sensor
const int SENSOR_READ = 1000;//Determina el tiempo de lectura del sensor a 1 segundo
const int LECTURE_THRESHOLD = 10; //Detecta un 10% de cambio en la humedad
const int FRAME_TIME = 2000; //Tiempo entre frames

//Variables
double timeNow, timeLast, timeFrame;//Variables de seguimiento de tiempo
byte ring = 0;//Indice del arreglo de anillo
int humidity [16];//Arreglo de anillo

//Constante de la bilbioteca que selecciona el modelo de sensor que usamos
#define DHTTYPE DHT11   // DHT 11
//#define DHTTYPE DHT22   // DHT 22  (AM2302), AM2321
//#define DHTTYPE DHT21   // DHT 21 (AM2301)

// Connect pin 1 (on the left) of the sensor to +5V
// NOTE: If using a board with 3.3V logic like an Arduino Due connect pin 1
// to 3.3V instead of 5V!
// Connect pin 2 of the sensor to whatever your DHTPIN is
// Connect pin 3 (on the right) of the sensor to GROUND (if your sensor has 3 pins)
// Connect pin 4 (on the right) of the sensor to GROUND and leave the pin 3 EMPTY (if your sensor has 4 pins)
// Connect a 10K resistor from pin 2 (data) to pin 1 (power) of the sensor

// Initialize DHT sensor.
// Note that older versions of this library took an optional third parameter to
// tweak the timings for faster processors.  This parameter is no longer needed
// as the current DHT reading algorithm adjusts itself to work on faster procs.
DHT dht(DHTPIN, DHTTYPE);

//Inicialización del programa, se ejecuta una sola vez al energizar o resetear
void setup() {
  //De momento no necesitamos comunicación serial, pero puede activarse para el debugging
  //Serial.begin(115200);
  //Serial.println(F("DHTxx test!"));

  //Objeto que inicia la comunicación con el sensor
  dht.begin();

  //Inicia secuencia de control de tiempos no bloqueante
  timeLast = millis (); //Esta función obtiene la hora del micro controlador
}

//Cuerpo de l prorgama. Este se es como un while infinito
void loop() {
  //Funcion de seguimiento de tiempo. Se recomienda dejar siempre al incio del loop
  timeNow = millis ();
  
  // Lectura del sensor

  if (timeNow > (timeLast + SENSOR_READ)) {
    
    // Reading temperature or humidity takes about 250 milliseconds!
    // Sensor readings may also be up to 2 seconds 'old' (its a very slow sensor)  
    
    //Esta es la función que lee el sensor. Esta se debe leer constantemente para detonar las generaciones.
    humidity [ring] = dht.readHumidity();

    //Comprobar el valor promedio almacenado en el anillo
    int avg;
    for (int i=0; i<16; i++;) {
      avg += humidity[i];
    }
    avg = avg/16;

    if ((avg - humidity [ring]) > LECTURE_THRESHOLD || (avg - humidity [ring]) < LECTURE_THRESHOLD) {
      doTheEvolutionBaby ();//Aqui es donde hay que poner el detonador de generaciones
    }

    //Secuencia del arreglo de anillo
    ring++;//Incremento del indice de anillo
    ring %= 16://Modulo del indice de anillo para limitar el valor del indice

    //Actualización de secuencias de tiempo
    timeLast = millis ();
  }

  //Cada que se cumpla el tiempo entre frames
  if (timeNow > (timeFrame + FRAME_TIME)) {
    //Calculo de frame


    /*Envio de frame. Para ello hay que seleccionar uno de los siguientes metodos:
     * 1. Llenar un arreglo de longitud 512. Ejemplo Multiple Strips in One Array
     * 2. Llenar una matriz de 8x8x8. Ejemplo MultiArrays
     * 3. Llenar 8 arreglos de longitud 64. Ejemplo Array of Led Arrays
     * 
     * Los ejemplos se encuentran en File>Examples>fastLEd>Multiple
     * Debe instalarse primero la biblioteca para poder verlos.
     * 
     * La lista de colores se encuentra en
     * https://github.com/FastLED/FastLED/blob/b5874b588ade1d2639925e4e9719fa7d3c9d9e94/src/colorpalettes.cpp
     * 
     * Para el ejemplo original usé Array of led Arrays
     * Cada array, para mostrarse, debe coronarse con:
     * FastLED.show();
     */

    //Esta la hace Yun con lo del automata
    calculador de frames ();

    //Esta la hace Hugo de acuerdo a la selección de Yun
    showLeds ();

    
    //Actualización de secuencia de tiempo
    timeFrame = millis ();
  }  
}

//Funciones de usuario

void doTheEvolutionBaby () {
  //Codigo de cambio generacional
}
