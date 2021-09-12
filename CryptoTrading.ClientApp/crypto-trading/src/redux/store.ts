import {createStore , Store} from 'redux';
import reducers from './reducers/index';
import {WalletAction,WalletState,DispatchType} from './types';


const store :Store<WalletState,WalletAction & {
    dispatch: DispatchType
}> = createStore(reducers,{}, (window as any).__REDUX_DEVTOOLS_EXTENSION__ && (window as any).__REDUX_DEVTOOLS_EXTENSION__());

export default store;