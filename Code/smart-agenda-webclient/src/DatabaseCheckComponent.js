import React, { useEffect, useState } from 'react';
import axios from 'axios';

function DatabaseCheckComponent() {
    const [connectionStatus, setConnectionStatus] = useState("");

    useEffect(() => {
        
        axios.get('https://localhost:7270/Health')
        .then(response => {
            setConnectionStatus(response.data);
        })
        .catch(error => {
            console.error('There was an error fetching data', error);
            setConnectionStatus('Failed to connect to the API');
        });
    }, []);  

    return (
        <div>
            <h2>Database Connection Status:</h2>
            <p>{connectionStatus}</p>
        </div>
    );
}

export default DatabaseCheckComponent;
