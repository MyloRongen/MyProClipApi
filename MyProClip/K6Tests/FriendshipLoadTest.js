import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    vus: 50,
    duration: '30s', 
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

    const endpointResponse = http.get('https://localhost:8000/api/friends', {
        headers: {
            Authorization: `Bearer ${authToken}`,
            'Content-Type': 'application/json'
        },
    });
    check(endpointResponse, {
        'get friends status is 200': (res) => res.status === 200,
    });

    sleep(4);
}