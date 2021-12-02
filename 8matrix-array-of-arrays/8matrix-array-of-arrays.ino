// ArrayOfLedArrays - see https://github.com/FastLED/FastLED/wiki/Multiple-Controller-Examples for more info on
// using multiple controllers.  In this example, we're going to set up three NEOPIXEL strips on three
// different pins, each strip getting its own CRGB array to be played with, only this time they're going
// to be all parts of an array of arrays.

#include <FastLED.h>

#define NUM_STRIPS 8
#define NUM_LEDS_PER_STRIP 64
CRGB leds[NUM_STRIPS][NUM_LEDS_PER_STRIP];

int wait = 5000;


// For mirroring strips, all the "special" stuff happens just in setup.  We
// just addLeds multiple times, once for each strip
void setup() {
  // tell FastLED there's 60 NEOPIXEL leds on pin D0
  FastLED.addLeds<NEOPIXEL, D0>(leds[0], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin D1
  FastLED.addLeds<NEOPIXEL, D1>(leds[1], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin D1
  FastLED.addLeds<NEOPIXEL, D2>(leds[2], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin D1
  FastLED.addLeds<NEOPIXEL, D3>(leds[3], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin D1
  FastLED.addLeds<NEOPIXEL, D4>(leds[4], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin D1
  FastLED.addLeds<NEOPIXEL, D5>(leds[5], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin D1
  FastLED.addLeds<NEOPIXEL, D6>(leds[6], NUM_LEDS_PER_STRIP);

  // tell FastLED there's 60 NEOPIXEL leds on pin D1
  FastLED.addLeds<NEOPIXEL, D7>(leds[7], NUM_LEDS_PER_STRIP);

  allBlack ();
  frame1 ();
  delay (10000);
  frame2 ();
  delay (wait);
  frame3 ();
  delay (wait);
  frame4 ();
  delay (wait);
  frame5 ();
  delay (wait);
  frame6 ();
  delay (wait);
  frame7 ();
  delay (wait);
  frame8 ();
  delay (wait);
 
}

void loop() {
  delay (1000);
}

void allBlack () {
  // This outer loop will go over each strip, one at a time
  for(int x = 0; x < NUM_STRIPS; x++) {
    // This inner loop will go over each led in the current strip, one at a time
    for(int i = 0; i < NUM_LEDS_PER_STRIP; i++) {
      leds[x][i] = CRGB::Black;
    }
  }
  FastLED.show();
  delay(10);
}

void frame1 () {
  leds [3][30] = CRGB:: Blue;
  leds [4][30] = CRGB:: Blue;
  leds [2][10] = CRGB:: Blue;
  FastLED.show();
}

void frame2 () {
  leds [1][1] = CRGB:: Blue;
  leds [0][55] = CRGB:: Blue;
  leds [4][12] = CRGB:: Blue;
  FastLED.show();
}

void frame3 () {
  leds [5][28] = CRGB:: Blue;
  leds [6][39] = CRGB:: Blue;
  leds [4][30] = CRGB:: Red;
  FastLED.show();
}

void frame4 () {
  leds [0][15] = CRGB:: Blue;
  leds [7][21] = CRGB:: Blue;
  leds [4][8] = CRGB:: Blue;
  leds [2][10] = CRGB:: Green;
  leds [4][31] = CRGB:: Red;
  FastLED.show();
}

void frame5 () {
  leds [2][47] = CRGB:: Blue;
  leds [0][53] = CRGB:: Blue;
  leds [5][11] = CRGB:: Blue;
  leds [3][31] = CRGB:: Red;
  leds [3][10] = CRGB:: Green;
  FastLED.show();
}

void frame6 () {
  leds [4][33] = CRGB:: Blue;
  leds [6][60] = CRGB:: Blue;
  leds [4][38] = CRGB:: Red;
  FastLED.show();
}

void frame7 () {
  leds [3][38] = CRGB:: Red;
  leds [2][9] = CRGB:: Green;
  FastLED.show();
}

void frame8 () {
  leds [3][37] = CRGB:: Red;
  FastLED.show();
}
