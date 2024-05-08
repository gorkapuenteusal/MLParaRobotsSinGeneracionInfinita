using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esta clase crea un filtro que genera montañas en los bordes del mapa, para que la dificultad de aprendizaje sea progresiva
public static class EdgeGenerator
{
  public static float[,] GenerateEdgeMap(int size) {
    float [,] map = new float[size, size];

    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
          // Valores de -1 a 1
          float x = i / (float) size * 2 - 1;
          float y = j / (float) size * 2 - 1;
    
          // Circunferencia más marcada en los 
          float value = Mathf.Clamp01(Mathf.Pow(x * x + y * y, 1 / 4f));

          map[i, j] = value;
        }
    }

    return map;
  }
}
