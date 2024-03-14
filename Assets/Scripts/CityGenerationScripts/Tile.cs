using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
   // Lists all the possible tiles that each neighbor can be
   public Tile[] upNeighbor;
   public Tile[] downNeighbor;
   public Tile[] leftNeighbor;
   public Tile[] rightNeighbor;
}