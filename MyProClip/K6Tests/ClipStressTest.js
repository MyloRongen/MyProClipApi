import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    stages: [
        { duration: '5s', target: 5 },
        { duration: '30s', target: 5 },
        { duration: '5s', target: 20 },
        { duration: '30s', target: 20 },
        { duration: '5s', target: 100 },
        { duration: '30s', target: 100 },
        { duration: '5s', target: 200 },
        { duration: '30s', target: 200 },
        { duration: '5s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<4000'],
    },
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
            'Content-Type': 'application/json',
        },
    });

    check(endpointResponse, {
        'get clips status is 200': (res) => res.status === 200,
    });

    sleep(3);
}