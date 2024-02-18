import ReactDOM from 'react-dom/client';
import 'semantic-ui-css/semantic.min.css';
import './layout/styles.css'
import { appStore, AppStoreContext } from './stores/appStore';
import { RouterProvider } from 'react-router-dom';
import { appRouter } from './router/AppRoutes';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(  
  <AppStoreContext.Provider value={appStore}>    
    <RouterProvider router={appRouter} />
  </AppStoreContext.Provider>
);