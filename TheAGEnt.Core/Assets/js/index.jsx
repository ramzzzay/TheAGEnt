import App from './containers/App';
import MainPage from './components/MainPage';
import AdminPanel from './components/AdminPanel';
import Registration from './components/Registration';
import Settings from './components/SettingsPage';
import UserAlbums from './components/UserAlbums';
import UserPhotos from './components/UserPhotos';
import { Router, Route, IndexRoute, browserHistory, hashHistory } from 'react-router'

ReactDOM.render(
  <Router history={browserHistory}>
    <Route path='/' component={App}>
      <IndexRoute component={MainPage} />
      <Route name='AdminPanel' path='AdminPanel' component={AdminPanel}/>
      <Route name='Registration' path='Registration' component={Registration}/>
      <Route name='Settings' path='Settings' component={Settings}/>
      <Route name='UserAlbums' path=':user' component={UserAlbums}/>
      <Route name='UserPhotos' path=':user/:userAlbumName' component={UserPhotos}/>
    </Route>
  </Router>,
  document.getElementById('root')
);

// ReactDOM.render(
//   <Router history={hashHistory}>
//     <Route path='/' component={App}>
//       <IndexRoute component={MainPage} />
//       <Route name='AdminPanel' path='AdminPanel' component={AdminPanel} handler={AdminPanel}/>
//       <Route name='Registration' path='Registration' component={Registration} handler={Registration}/>
//       <Route name='Settings' path='Settings' component={Settings} handler={Settings}/>
//     </Route>
//   </Router>,
//   document.getElementById('root')
// );
