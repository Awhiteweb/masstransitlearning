#!/bin/bash

trigger() {
    curl "https://localhost:8081/book" \
        -X POST \
        -H "Content-Type: application/json" \
        -d "{\"matchDate\":\"$1\",\"from\":\"test\"}" \
        -k
}

trigger "2024-01-02T10:10:00"
echo ""
trigger "2024-01-03T10:10:00"
echo ""
trigger "2024-01-04T10:10:00"
echo ""
trigger "2024-01-05T10:10:00"
echo ""
trigger "2024-01-06T10:10:00"
echo ""
trigger "2024-01-07T10:10:00"
echo ""