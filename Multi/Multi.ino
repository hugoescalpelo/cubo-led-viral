// ArrayOfLedArrays - see https://github.com/FastLED/FastLED/wiki/Multiple-Controller-Examples for more info on
// using multiple controllers.  In this example, we're going to set up three NEOPIXEL strips on three
// different pins, each strip getting its own CRGB array to be played with, only this time they're going
// to be all parts of an array of arrays.

#include "DHT.h"
#include <FastLED.h>

FASTLED_USING_NAMESPACE

#define NUM_STRIPS 8
#define NUM_LEDS_PER_STRIP 64
//CRGB leds[NUM_STRIPS][NUM_LEDS_PER_STRIP];
CRGB leds0[NUM_LEDS_PER_STRIP];
CRGB leds1[NUM_LEDS_PER_STRIP];
CRGB leds2[NUM_LEDS_PER_STRIP];
CRGB leds3[NUM_LEDS_PER_STRIP];
CRGB leds4[NUM_LEDS_PER_STRIP];
CRGB leds5[NUM_LEDS_PER_STRIP];
CRGB leds6[NUM_LEDS_PER_STRIP];
CRGB leds7[NUM_LEDS_PER_STRIP];
#define LED_TYPE    WS2811
#define COLOR_ORDER GRB
#define BRIGHTNESS          32
#define FRAMES_PER_SECOND  120
uint8_t gHue0 = 0; // rotating "base color" used by many of the patterns
uint8_t gHue1 = 0;
uint8_t gHue2 = 0;
uint8_t gHue3 = 0;
uint8_t gHue4 = 0;
uint8_t gHue5 = 0;
uint8_t gHue6 = 0;
uint8_t gHue7 = 0;
#define DHTPIN D7//Pin donde se lee el sensor
const int SENSOR_READ = 250;//Determina el tiempo de lectura del sensor a 1 segundo
const int LECTURE_THRESHOLD = 3; //Detecta un 10% de cambio en la humedad
int avg;

//Variables
double timeNow, timeLast, timeFrame;//Variables de seguimiento de tiempo
byte ring = 0;//Indice del arreglo de anillo
int humidity [16];//Arreglo de anillo

//Constante de la bilbioteca que selecciona el modelo de sensor que usamos
#define DHTTYPE DHT11   // DHT 11

int pos;

DHT dht(DHTPIN, DHTTYPE);
// For mirroring strips, all the "special" stuff happens just in setup.  We
// just addLeds multiple times, once for each strip
void setup() {
 
  FastLED.addLeds<LED_TYPE,D0,COLOR_ORDER>(leds0, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);
  FastLED.addLeds<LED_TYPE,D1,COLOR_ORDER>(leds1, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);
  FastLED.addLeds<LED_TYPE,D2,COLOR_ORDER>(leds2, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);
  FastLED.addLeds<LED_TYPE,D3,COLOR_ORDER>(leds3, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);
  FastLED.addLeds<LED_TYPE,D4,COLOR_ORDER>(leds4, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);
  FastLED.addLeds<LED_TYPE,D5,COLOR_ORDER>(leds5, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);
  FastLED.addLeds<LED_TYPE,D6,COLOR_ORDER>(leds6, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);
  FastLED.addLeds<LED_TYPE,D8,COLOR_ORDER>(leds7, NUM_LEDS_PER_STRIP).setCorrection(TypicalLEDStrip);

  //Serial.begin (115200);
  //Serial.println ("Inicio");

  dht.begin();
  //Inicia secuencia de control de tiempos no bloqueante
  timeLast = millis (); //Esta función obtiene la hora del micro controlador
}

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
    
    for (int i=0; i<15; i++) {
      avg += humidity[i];
    }
    avg = avg/16;

    //Serial.print ("avg ");
    //Serial.print (avg);
    //Serial.print (" hum ");
    //Serial.println (humidity[ring]);

    if ((humidity [ring] - avg) > LECTURE_THRESHOLD) {
      doTheEvolutionBaby ();//Aqui es donde hay que poner el detonador de generaciones
    }

    //Secuencia del arreglo de anillo
    ring++;//Incremento del indice de anillo
    ring %= 16;//Modulo del indice de anillo para limitar el valor del indice

    //Actualización de secuencias de tiempo
    timeLast = millis ();
  }
  confetti ();
  // send the 'leds' array out to the actual LED strip
  FastLED.show();  
  // insert a delay to keep the framerate modest
  FastLED.delay(5000/FRAMES_PER_SECOND); 

  // do some periodic updates
  //EVERY_N_MILLISECONDS( 5000 ) { gHue += random8(255); }
}

void confetti() 
{
  // random colored speckles that blink in and fade smoothly
  fadeToBlackBy( leds0, NUM_LEDS_PER_STRIP, 40);
  fadeToBlackBy( leds1, NUM_LEDS_PER_STRIP, 40);
  fadeToBlackBy( leds2, NUM_LEDS_PER_STRIP, 40);
  fadeToBlackBy( leds3, NUM_LEDS_PER_STRIP, 40);
  fadeToBlackBy( leds4, NUM_LEDS_PER_STRIP, 40);
  fadeToBlackBy( leds5, NUM_LEDS_PER_STRIP, 40);
  fadeToBlackBy( leds6, NUM_LEDS_PER_STRIP, 40);
  fadeToBlackBy( leds7, NUM_LEDS_PER_STRIP, 40);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds0[pos] += CHSV( gHue0 + random8(32), 200, 255);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds1[pos] += CHSV( gHue1 + random8(32), 200, 255);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds2[pos] += CHSV( gHue2 + random8(32), 200, 255);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds3[pos] += CHSV( gHue3 + random8(32), 200, 255);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds4[pos] += CHSV( gHue4 + random8(32), 200, 255);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds5[pos] += CHSV( gHue5 + random8(32), 200, 255);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds6[pos] += CHSV( gHue6 + random8(32), 200, 255);
  pos = random16(NUM_LEDS_PER_STRIP);
  leds7[pos] += CHSV( gHue7 + random8(32), 200, 255);
  //addGlitter(10);
}

void doTheEvolutionBaby () {
  //
  gHue0 += random8(64);
  gHue1 += random8(64);
  gHue2 += random8(64);
  gHue3 += random8(64);
  gHue4 += random8(64);
  gHue5 += random8(64);
  gHue6 += random8(64);
  gHue7 += random8(64);
}

void addGlitter( fract8 chanceOfGlitter) 
{
  if( random8() < chanceOfGlitter) {
    leds6[ random16(NUM_LEDS_PER_STRIP) ] += CRGB::White;
  }
}
