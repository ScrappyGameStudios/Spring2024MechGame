using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
   public bool collapsed; // If a cell has been collapsed or not
   public Tile[] tileOptions; // Contains all the options a cell can currently be. Will update during instantiation

   public void CreateCell(bool collapseState, Tile[] tiles) {
      collapsed = collapseState;
      tileOptions = tiles;
   }

   public void RecreateCell(Tile[] tiles) {
      tileOptions = tiles;
   }
}
