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
      pictures: []
    };
  },
  getPictures: function() {
    fetch(`/api/Photos/GetUserPhotosByNickNameAndAlbumName?nickname=${Cookie.load('nickname')}&albumName=${this.props.params.userAlbumName}`, {
        method: 'GET',
        headers: new Headers({
            "Content-Type": "application/json",
            "Authorization": "bearer " + Cookie.load('tokenInfo')
        })
        }).then(r => r.json()).then(a => this.setState({pictures: Array.from(a)}));
  },
  componentDidMount: function() {
    this.getPictures();
  },
  render: function() {
    return (
      <div style={styles.root}>
          <GridList cellHeight={180} style={styles.gridList}>
              <Subheader>Albums</Subheader>
              {this.state.pictures.map((picture) => (
                  <GridTile title={picture.Label} subtitle={< span > < b > {
                      picture.Discription
                  } < /b></span >} actionIcon={<FlatButton>This is photo</FlatButton>}>
                      <img src={picture.PathToImage}/>
                  </GridTile>
              ))}
          </GridList>
      </div>
    );
  }
});
