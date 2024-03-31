import axios from 'axios';

//create the axios instance
if (localStorage.getItem('token')) {
    axios.defaults.headers.common['Authorization'] = `Bearer ${localStorage.getItem('token')}`;
}

//api base should follow http or https depending on the current protocol
const API_BASE_URL = 'http://localhost:44357/api';


// Function to set the token in localStorage and Axios headers
export const setAuthToken = (token) => {
    if (token) {
        localStorage.setItem('token', token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    } else {
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization'];
    }
};

//setup the interceptor to check for 401 status
axios.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response && error.response.status === 401 && error.config.url !== `${API_BASE_URL}/Authorization/me`) {
            setAuthToken(null);
            window.location.href = '/?msg=loggedOut';
            console.log(error);
        }
        return Promise.reject(error);
    }
);

export const login = async (username) => {
    const response = await axios.post(`${API_BASE_URL}/Authorization/login?userName=${username}`);
    if (response.data) {
        setAuthToken(response.data);
    }
    return response;
};

export const getUserInfo = async () => {
    //if the result is 401, it will be caught by the interceptor
    return await axios.get(`${API_BASE_URL}/Authorization/me`);
};

export const getClanList = () => {
    return axios.get(`${API_BASE_URL}/Clan/list`);
};

export const getClanDetails = (clanId) => {
    return axios.get(`${API_BASE_URL}/Clan/${clanId}`);
}

export const joinClan = (clanId) => {
    return axios.post(`${API_BASE_URL}/Clan/${clanId}/join`);
};

export const leaveClan = (clanId) => {
    return axios.post(`${API_BASE_URL}/Clan/${clanId}/leave`);
};

export const updateClanPoints = (clanId, points, action) => {
    return axios.post(`${API_BASE_URL}/Clan/${clanId}/points/${action}/${points}`);
}

export const getContributions = (clanId) => {
    return axios.get(`${API_BASE_URL}/Clan/${clanId}/contributions`);
};
