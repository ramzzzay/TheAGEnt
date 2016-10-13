import {GridList, GridTile} from 'material-ui/GridList';
import IconButton from 'material-ui/IconButton';
import Subheader from 'material-ui/Subheader';
import StarBorder from 'material-ui/svg-icons/toggle/star-border';

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
    return {
      albums: []
    };
  },
  getAlbums: function() {
    fetch(`api/Photos/GetUserAlbumsNameByNickName?nickname=${this.props.params.user}`, {
        method: 'GET',
        headers: new Headers({
            "Content-Type": "application/json",
            "Authorization": "bearer " + Cookie.load('tokenInfo')
        })
        }).then(r => r.json()).then(a => this.setState({albums: Array.from(a)}));
  },
  componentDidMount: function() {
    this.getAlbums();
    Cookie.save('nickname', this.props.params.user);
  },
  render: function() {
    console.log(this.state.albums);
    return (
      <div style={styles.root}>
          <GridList cellHeight={180} style={styles.gridList}>
              <Subheader>Albums</Subheader>
              {this.state.albums.map((album) => (
                  <GridTile key={album.img} title={album.Name} subtitle={< span > by < b > {
                      this.props.params.user
                  } < /b></span >} actionIcon={<FlatButton href={`${this.props.params.user}/${album.Name}`}>React Router</FlatButton>}>
                      <img src={album.PathToPhoto}/>
                  </GridTile>
              ))}
          </GridList>
      </div>
    );
  }
});﻿
