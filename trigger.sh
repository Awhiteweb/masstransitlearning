#!/bin/bash

trigger() {
    curl "https://localhost:8081/book" \
        -X POST \
        -H "Content-Type: application/json" \
        -d "{\"matchDate\":\"$1\",\"from\":\"test\"}" \
        -k
}

trigger $(date +"%FT%T")
echo ""
# trigger $(date -d "tomorrow" +"%FT%T")
# echo ""
# trigger $(date -d "next week" +"%FT%T")
# echo ""