import {GridList, GridTile} from 'material-ui/GridList';
import Subheader from 'material-ui/Subheader';

import FlatButton from 'material-ui/FlatButton';
import TextField from 'material-ui/TextField';

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

var Search = React.createClass({
    getInitialState: function () {
        return {
            search:""
        };
    },
    _searchFieldChange: function (e) {
        this.setState({search: e.target.value});
        this.props.setUsers(this.props.users.filter(x=>x.Name.includes(e.target.value)),e.target.value);
    },
    render: function () {
        return (
            <TextField
                value={this.state.search}
                onChange={this._searchFieldChange}
                hintText="Search"
                fullWidth={true}
            />
        );
    }
});


module.exports = React.createClass({
    getInitialState: function() {
        return {users: [],immutableUsers:[]};
    },
    getUsers: function() {
      fetch('api/Account/GetAllUsersMiniInfo', {
          method: 'GET',
          headers: new Headers({
              'Authorization': "bearer " + Cookie.load('tokenInfo'),
              'Content-Type': "application/json"
          })
      }).then(r => r.json()).then(data => {
          this.setState({users:data,immutableUsers:data})
      });
    },
    setUsers: function (filteredUsers,text) {
        if( (filteredUsers == false && !text) || !text ){
            this.setState({users:this.state.immutableUsers})
        }
        else{
            this.setState({users:filteredUsers})
        }
    },
    componentDidMount: function() {
      this.getUsers();
    },
    render: function() {
        return (
            <div style={styles.root}>
                <Search users={this.state.users} setUsers={this.setUsers}/>
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
