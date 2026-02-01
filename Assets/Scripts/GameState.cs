public enum GameState
{
    FreeLook,       // Spieler kann sich umsehen und interagieren
    Dialogue,       // Spieler ist im Gespräch gefangen (Camera Lock)
    ReadingLog,     // Spieler starrt auf den Screen (UI Focus)
    Cutscene,       // Z.B. das Umdrehen zur Tür
    GameOver        // Endscreen
}