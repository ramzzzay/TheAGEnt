import App from './containers/App';
import MainPage from './components/MainPage';
import AdminPanel from './components/AdminPanel';
import Registration from './components/Registration';
import Settings from './components/SettingsPage';
import { Router, Route, IndexRoute, browserHistory, HistoryLocation, hashHistory } from 'react-router'


ReactDOM.render(
  <Router history={hashHistory}>
    <Route path='/' component={App}>
      <IndexRoute component={MainPage} />
      <Route name='AdminPanel' path='AdminPanel' component={AdminPanel} handler={AdminPanel}/>
      <Route name='Registration' path='Registration' component={Registration} handler={Registration}/>
      <Route name='Settings' path='Settings' component={Settings} handler={Settings}/>
    </Route>
  </Router>,
  document.getElementById('root')
);
