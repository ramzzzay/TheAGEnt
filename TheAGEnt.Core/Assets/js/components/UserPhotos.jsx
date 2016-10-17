import {GridList, GridTile} from 'material-ui/GridList';
import Subheader from 'material-ui/Subheader';

import Dialog from 'material-ui/Dialog';
import RaisedButton from 'material-ui/RaisedButton';
import TextField from 'material-ui/TextField';

import IconButton from 'material-ui/IconButton';
import {cyan700} from 'material-ui/styles/colors';
import SendIcon from 'material-ui/svg-icons/content/send';

const customContentStyle = {
    display: 'flex',
    maxWidth: 'auto'
};

const Full_Image = React.createClass({
    getInitialState: function() {
        return {comments: [],message:""};
    },
    getComments: function() {
        fetch(`/api/Photos/GetCommentsToPhotoById?nickname=${this.props.nicknameOfPhotoOwner}&albumName=${this.props.userAlbumName}&photoId=${this.props.imageId}`, {
            method: 'GET',
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load('tokenInfo')
            })
        }).then(r => r.json()).then(c => this.setState({comments: Array.from(c)}));
    },
    cleanForm: function () {
      this.setState({message:""});
    },
    sendMessage: function () {
        var data = {
            NickNameOfPhotoOwner: this.props.nicknameOfPhotoOwner,
            NickNameOfSender: this.props.nickNameOfSender,
            AlbumName: this.props.userAlbumName,
            PhotoId:this.props.imageId,
            Message: this.state.message
        };
        fetch("/api/Photos/SendComment", {
            method: 'POST',
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load('tokenInfo')
            }),
            body: JSON.stringify(data)
        }).then(()=>{
            this.getComments();
            this.cleanForm();
        });
    },
    _messageFieldChange: function(e) {
        this.setState({message: e.target.value});
    },
    componentDidUpdate : function (prevProps, prevState) {
        if(this.props.open != prevProps.open){
            this.getComments();
        }
    },
    render: function() {
        return (
            <Dialog contentStyle={customContentStyle} title={this.props.title} modal={this.props.modal} open={this.props.open} onRequestClose={this.props.onRequestClose}>
                <div className="container-with-comments">
                <div ref="Image" className="Comments">
                    <img className="Full-Image" src={this.props.imageUrl}/>
                </div>
                <div className="Comments-box">
                    <div className="Comments-List">
                    <span>{this.state.comments.length}</span> Comments
                    {this.state.comments.map(c=>(
                        <div className="Message-from-user">{c.NickName} say:  {c.Message} in {c.PostingTime}</div>
                    ))}
                    </div>
                    <div className="Comment-send">
                        <TextField value={this.state.message} onChange={this._messageFieldChange} hintText="Enter message" multiLine={true}/>
                        <IconButton tooltip="Send" touch={true} onClick={this.sendMessage} tooltipPosition="bottom-center">
                            <SendIcon color={cyan700}/>
                        </IconButton>
                    </div>
                </div>
                </div>
            </Dialog>
        );
    }
});

const styles = {
    root: {
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'space-around'
    }
};

module.exports = React.createClass({
  getInitialState: function() {
    return {
      pictures: [],
        openWall: false,
        PathToClickedImage: "",
        ClickedImageId: ""
    };
  },
  getPictures: function() {
    fetch(`/api/Photos/GetUserPhotosByNickNameAndAlbumName?nickname=${this.props.params.user}&albumName=${this.props.params.userAlbumName}`, {
        method: 'GET',
        headers: new Headers({
            "Content-Type": "application/json",
            "Authorization": "bearer " + Cookie.load('tokenInfo')
        })
        }).then(r => r.json()).then(a => this.setState({pictures: Array.from(a)}));
  },
    handleOpenWall: function (e) {
      this.setState({openWall: true,
          PathToClickedImage:e.target.src,
          ClickedImageId: e.target.id});
    },
    handleCloseWall: function () {
        this.setState({openWall: false});
    },
  componentDidMount: function() {
    this.getPictures();
  },
  render: function() {
    return (
      <div style={styles.root}>
          <GridList cellHeight={180}>
              <Subheader>Albums</Subheader>
              {this.state.pictures.map((picture) => (
                  <GridTile title={picture.Label} subtitle={< span > < b > {
                      picture.Discription
                  } </b></span >}>
                      <img id={picture.Id} onClick={this.handleOpenWall} src={picture.PathToImage}/>
                  </GridTile>
              ))}
              <Full_Image userAlbumName={this.props.params.userAlbumName} nicknameOfPhotoOwner={this.props.params.user} nickNameOfSender={Cookie.load('nickname')} imageId={this.state.ClickedImageId} imageUrl={this.state.PathToClickedImage} title="Full information" modal={false} open={this.state.openWall} onRequestClose={this.handleCloseWall}/>
          </GridList>
      </div>
    );
  }
});
