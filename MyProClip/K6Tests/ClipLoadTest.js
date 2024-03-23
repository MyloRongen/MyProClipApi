import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    vus: 200,
    duration: '1s', 
};  

export default function () {
    const loginPayload = JSON.stringify({
        email: 'Mypro',
        password: 'Password123!',
      });
    
      const loginResponse = http.post('https://localhost:8000/login', loginPayload, {
        headers: {
          'Content-Type': 'application/json',
        },
      });

    check(loginResponse, {
        'login status is 200': (res) => res.status === 200,
    });

    const authToken = loginResponse.json('accessToken');

    const endpointResponse = http.get('https://localhost:8000/api/clips', {
        headers: {
            Authorization: `Bearer ${authToken}`,
            'Content-Type': 'application/json'
        },
    });
    
    check(endpointResponse, {
        'get clips status is 200': (res) => res.status === 200,
    });

    sleep(4);
}