import {combineReducers} from 'redux';
import {walletReducer} from '../reducers/walletReducer';

const reducers = combineReducers ({
   wallet:walletReducer 
});

export default reducers;