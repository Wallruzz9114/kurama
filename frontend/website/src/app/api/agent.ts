import axios, { AxiosResponse } from 'axios';
import { toast } from 'react-toastify';
import { history } from '../..';
import { IActivity } from '../models/activity';

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.response.use(undefined, (error) => {
  const { status, data, config } = error.response;

  if (error.message === 'Network Error' && !error.response) {
    toast.error('Network error - server down');
  }

  if (status === 404) {
    history.push('/notfound');
  }

  if (status === 400 && config.method === 'get' && data.errors.hasOwnProperty('id')) {
    history.push('/notfound');
  }

  if (status === 500) {
    toast.error("Server error - we're looking into it");
  }
});

const controllers = {
  activities: '/activities',
};
const responseBody = (axiosrResponse: AxiosResponse) => axiosrResponse.data;
const sleep = (ms: number) => (response: AxiosResponse) =>
  new Promise<AxiosResponse>((resolve) => setTimeout(() => resolve(response), ms));

const requests = {
  get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(sleep(1000)).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(sleep(1000)).then(responseBody),
  delete: (url: string) => axios.delete(url).then(sleep(1000)).then(responseBody),
};

const ActivitiesService = {
  listAll: (): Promise<IActivity[]> => requests.get(controllers.activities),
  getOne: (id: string) => requests.get(controllers.activities + `/${id}`),
  create: (activity: IActivity) => requests.post(controllers.activities, activity),
  update: (activity: IActivity) =>
    requests.put(controllers.activities + `/${activity.id}`, activity),
  delete: (id: string) => requests.delete(controllers.activities + `/${id}`),
};

export default ActivitiesService;
