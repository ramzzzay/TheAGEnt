import App from './containers/App';
import MainPage from './components/MainPage';
import AdminPanel from './components/AdminPanel';
import Registration from './components/Registration';
import Settings from './components/SettingsPage';
import { Router, Route, IndexRoute, browserHistory } from 'react-router'


ReactDOM.render(
  <Router history={browserHistory}>
    <Route path='/' component={App}>
      <IndexRoute component={MainPage} />
      <Route path='AdminPanel' component={AdminPanel} />
      <Route path='Registration' component={Registration} />
      <Route path='Settings' component={Settings} />
    </Route>
  </Router>,
  document.getElementById('root')
);
