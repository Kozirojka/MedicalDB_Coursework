import { BASE_API } from '../../constants/BASE_API';

export const fetchVisitPendingRequests = async () => {
    try {
        const token = localStorage.getItem('accessToken');
        const response = await fetch(`${BASE_API}/v2/doctor/visits/pending-visits`, {
            method: 'GET',
            
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        console.log('Response:', response); // 🔍 Для дебагу

        if (!response.ok) {
            throw new Error('Failed to fetch requests');
        }

        const contentType = response.headers.get('Content-Type');
        const contentLength = response.headers.get('Content-Length');

        if ((contentLength === '0') || !contentType?.includes('application/json')) {
            return [];
        }

        const text = await response.text();

        if (!text.trim()) {
            return [];
        }

        const data = JSON.parse(text);
        return data;

    } catch (error) {
        console.error('Error fetching pending requests:', error);
        throw error;
    }
};
