import axios from 'axios';

import {serviceConfig}  from '../appSettings';

export default axios.create({
  baseURL: serviceConfig.baseURL
});