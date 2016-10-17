import {GridList, GridTile} from 'material-ui/GridList';
import Subheader from 'material-ui/Subheader';

import FlatButton from 'material-ui/FlatButton';

const styles = {
    root: {
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'space-around'
    },
    gridList: {
        width: 500,
        height: 450,
        overflowY: 'auto'
    }
};

module.exports = React.createClass({
    getInitialState: function() {
        return {users: []};
    },
    getUsers: function() {
      fetch('api/Account/GetAllUsersMiniInfo', {
          method: 'GET',
          headers: new Headers({
              'Authorization': "bearer " + Cookie.load('tokenInfo'),
              'Content-Type': "application/json"
          })
      }).then(r => r.json()).then(data => {
          this.setState({users:data})
      });
    },
    componentDidMount: function() {
      this.getUsers();
    },
    render: function() {
        return (
            <div style={styles.root}>
                <GridList cellHeight={180}>
                    <Subheader>Users</Subheader>
                    {this.state.users.map((user) => (
                        <GridTile key={user.img} title={user.Email} subtitle={< span > by < b > {
                            user.Name
                        } </b></span >} actionIcon={<FlatButton label="Pick" href={`#${user.NickName}`}/>}>
                            <img src={user.PathToPhoto}/>
                        </GridTile>
                    ))}
                </GridList>
            </div>
        );
    }
});
