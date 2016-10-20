import App from './containers/App';
import MainPage from './components/MainPage';
import AdminPanel from './components/AdminPanel';
import Settings from './components/SettingsPage';
import UserAlbums from './components/UserAlbums';
import UserPhotos from './components/UserPhotos';
import { Router, Route, IndexRoute, hashHistory } from 'react-router'

ReactDOM.render(
  <Router history={hashHistory}>
    <Route path='/' component={App}>
      <IndexRoute component={MainPage} />
      <Route name='AdminPanel' path='AdminPanel' component={AdminPanel} handler={AdminPanel}/>
      <Route name='Settings' path='Settings' component={Settings} handler={Settings}/>
      <Route name='UserAlbums' path=':user' handler={UserAlbums} component={UserAlbums}/>
      <Route name='UserPhotos' path=':user/:userAlbumName' handler={UserPhotos} component={UserPhotos}/>
    </Route>
  </Router>,
  document.getElementById('root')
);
