import {GridList, GridTile} from "material-ui/GridList";
import Subheader from "material-ui/Subheader";
import Dialog from "material-ui/Dialog";
import TextField from "material-ui/TextField";
import IconButton from "material-ui/IconButton";
import {cyan700} from "material-ui/styles/colors";
import SendIcon from "material-ui/svg-icons/content/send";
import {RadioButton, RadioButtonGroup} from "material-ui/RadioButton";
import ActionFavorite from "material-ui/svg-icons/action/stars";
import ActionFavoriteBorder from "material-ui/svg-icons/action/favorite-border";

const customContentStyle = {
    dialog: {
        display: "flex",
        maxWidth: "auto"
    }
};

const styles = {
    root: {
        display: "flex",
        flexWrap: "wrap",
        justifyContent: "space-around"
    },
    st: {
        display: "flex"
    },
    ar: {
        display: "flex",
        flexDirection: "row"
    }
};

const Full_Image = React.createClass({
    getInitialState: function () {
        return {
            comments: [],
            message: "",
            graded: false,
            grade: 0
        };
    },
    starsComponents: function () {
        var arr = [];
        for (var i = 1; i <= 5; i++) {
            arr.push(<RadioButton
                key={i}
                value={i}
                label={i}
                checkedIcon={<ActionFavorite />}
                uncheckedIcon={<ActionFavoriteBorder />}
                style={styles.radioButton}
            />)
        }
        return (
            <RadioButtonGroup onChange={this.sendGrade} style={styles.ar} name="stars">
                {arr}
            </RadioButtonGroup>
        );
    },
    getComments: function () {
        fetch(`/api/Photos/GetCommentsToPhotoById?nickname=${this.props.nicknameOfPhotoOwner}&albumName=${this.props.userAlbumName}&photoId=${this.props.imageId}`, {
            method: "GET",
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load("tokenInfo")
            })
        })
            .then(r => r.json())
            .then(c => this.setState({comments: Array.from(c)}));
    },
    cleanForm: function () {
        this.setState({message: ""});
    },
    sendGrade: function (event, value) {
        console.log("log from grades", event, value);
        var data = {
            PhotoOwner: this.props.nicknameOfPhotoOwner,
            SenderNickname: this.props.SenderNickname,
            AlbumName: this.props.userAlbumName,
            PhotoId: this.props.imageId,
            NumberOfGrade: value,
            Graded: true
        };
        fetch("/api/Photos/SetGradesAsync", {
            method: "POST",
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load("tokenInfo")
            }),
            body: JSON.stringify(data)
        }).then(()=> {
            this.setState({
                graded: true
            })
        });
    },
    currentGrade: function () {
        fetch(`/api/Photos/GetGrades?photoOwner=${this.props.nicknameOfPhotoOwner}&albumName=${this.props.userAlbumName}&photoId=${this.props.imageId}`, {
            method: "GET",
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load("tokenInfo")
            })
        })
            .then(r => r.json())
            .then(g => this.setState({grade: g}));
    },
    sendMessage: function () {
        var data = {
            NickNameOfPhotoOwner: this.props.nicknameOfPhotoOwner,
            SenderNickname: this.props.SenderNickname,
            AlbumName: this.props.userAlbumName,
            PhotoId: this.props.imageId,
            Message: this.state.message
        };
        fetch("/api/Photos/SendComment", {
            method: "POST",
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load("tokenInfo")
            }),
            body: JSON.stringify(data)
        }).then(()=> {
            this.getComments();
            this.cleanForm();
        });
    },
    _messageFieldChange: function (e) {
        this.setState({message: e.target.value});
    },
    gradedStatus: function () {
        fetch(`/api/Photos/GradedCheck?photoOwnerNickname=${this.props.nicknameOfPhotoOwner}&nickname=${this.props.SenderNickname}&albumName=${this.props.userAlbumName}&photoId=${this.props.imageId}`, {
            method: "GET",
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load("tokenInfo")
            })
        })
            .then(r => r.json())
            .then(g => this.setState({graded: g}));
    },
    componentDidMount: function () {
        this.getComments();
        this.gradedStatus();
        this.currentGrade();
    },
    render: function () {
        return (
            <Dialog contentStyle={customContentStyle.dialog} title={this.props.title} modal={this.props.modal}
                    open={this.props.open} onRequestClose={this.props.onRequestClose}>
                <div className="container-with-comments">
                    <div ref="Image" className="Comments">
                        <img className="Full-Image" src={this.props.imageUrl}/>
                    </div>
                    <div className="Comments-box">
                        <div className="Comments-List">
                            <span>{this.state.comments.length}</span> Comments
                            {this.state.comments.map(c=>(
                                <div className="Message-from-user">{c.NickName} say: {c.Message}
                                    in {c.PostingTime}</div>
                            ))}
                        </div>
                        <div className="Comment-send">
                            <TextField value={this.state.message} onChange={this._messageFieldChange}
                                       hintText="Enter message" multiLine={true}/>
                            <IconButton tooltip="Send" touch={true} onClick={this.sendMessage}
                                        tooltipPosition="bottom-center">
                                <SendIcon color={cyan700}/>
                            </IconButton>
                        </div>
                        <div style={styles.st} className="stars">
                            {this.state.graded ? <div>{this.state.grade}</div> : this.starsComponents()}
                        </div>
                    </div>
                </div>
            </Dialog>
        );
    }
});


module.exports = React.createClass({
    getInitialState: function () {
        return {
            pictures: [],
            openWall: false,
            PathToClickedImage: "",
            ClickedImageId: ""
        };
    },
    getPictures: function () {
        fetch(`/api/Photos/GetUserPhotosByNickNameAndAlbumName?nickname=${this.props.params.user}&albumName=${this.props.params.userAlbumName}`, {
            method: "GET",
            headers: new Headers({
                "Content-Type": "application/json",
                "Authorization": "bearer " + Cookie.load("tokenInfo")
            })
        }).then(r => r.json()).then(a => this.setState({pictures: Array.from(a)}));
    },
    handleOpenWall: function (e) {
        this.setState({
            openWall: true,
            PathToClickedImage: e.target.src,
            ClickedImageId: e.target.id
        });
    },
    handleCloseWall: function () {
        this.setState({openWall: false});
    },
    componentDidMount: function () {
        this.getPictures();
    },
    fullImage: function () {
        return (
            <Full_Image userAlbumName={this.props.params.userAlbumName}
                        nicknameOfPhotoOwner={this.props.params.user} SenderNickname={Cookie.load("nickname")}
                        imageId={this.state.ClickedImageId} imageUrl={this.state.PathToClickedImage}
                        title="Full information" modal={false} open={this.state.openWall}
                        onRequestClose={this.handleCloseWall}/>
        )
    },
    render: function () {
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
                    {this.state.openWall ? this.fullImage() : <div></div>}
                </GridList>
            </div>
        );
    }
});
