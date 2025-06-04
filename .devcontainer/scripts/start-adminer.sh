#!/bin/bash
echo "🗄️ Starting Adminer..."

# Prüfen ob PHP verfügbar ist
if ! command -v php &> /dev/null; then
    echo "❌ PHP ist nicht installiert!"
    exit 1
fi

# Prüfen ob Adminer existiert
if [ ! -f "/opt/adminer/adminer.php" ]; then
    echo "❌ Adminer nicht gefunden - lade herunter..."
    mkdir -p /opt/adminer
    wget -O /opt/adminer/adminer.php https://www.adminer.org/latest.php
fi

echo "🚀 Starte Adminer auf http://localhost:8090"
echo "💡 Drücke Ctrl+C zum Beenden"
php -S 0.0.0.0:8090 -t /opt/adminer /opt/adminer/adminer.php