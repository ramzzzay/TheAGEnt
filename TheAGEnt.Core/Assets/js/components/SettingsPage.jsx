import {
    Card,
    CardActions,
    CardHeader,
    CardMedia,
    CardTitle,
    CardText
} from 'material-ui/Card';
import {Tabs, Tab} from 'material-ui/Tabs';
import SwipeableViews from 'react-swipeable-views';
import ImageUpload from "./ImageUpload.jsx";
import UpdateUserInfo from "./UpdateUserInfo.jsx";
import AlbumsUpdate from "./AlbumsUpdate.jsx";
import Avatar from 'material-ui/Avatar';

const Av = React.createClass({
    render: function () {
        return (
            <div>
                Hello to
                <Avatar src={this.props.pathToPhoto} size="200" />
            </div>
        );
    }
});

const styles = {
    headline: {
        fontSize: 24,
        paddingTop: 16,
        marginBottom: 12,
        fontWeight: 400
    },
    slide: {
        padding: 10
    },
    avatar:{
        position: 'absolute',
        zIndex:1,
        left:'50%'
    }
};

module.exports = React.createClass({
    getInitialState: function() {
        return {
            name: "",
            surname: "",
            nickname: "",
            address: "",
            pathToPhoto: "",
            pathToCard: "",
            slideIndex: 0
        };
    },
    handleChange: function(value) {
        this.setState({slideIndex: value});
    },
    loadFromServer: function() {
        fetch('api/Account/AllUserInfo', {
            method: 'GET',
            headers: new Headers({
                'Authorization': "bearer " + Cookie.load('tokenInfo'),
                'Content-Type': "application/json"
            })
        }).then(r => r.json()).then(data => {
            this.setState({
                email: data.Email,
                name: data.Name,
                surname: data.Surname,
                nickname: data.NickName,
                address: data.Address,
                pathToPhoto: data.PathToPhoto,
                pathToCard: data.PathToCard,
            })
        });
    },
    changingUserInfo: function(newData){
      this.setState({
        email: newData.Email || this.state.email,
        name: newData.Name || this.state.name,
        surname: newData.Surname || this.state.surname,
        nickname: newData.NickName || this.state.nickname,
        address: newData.address || this.state.address
      });
    },
    changingPathToAvatar:function(path){
      this.setState({pathToPhoto: path});
    },
    changingPathToCard:function(path){
      this.setState({pathToCard: path});
    },
    componentDidMount: function() {
        this.loadFromServer();
    },
    render: function() {
        return (
            <div ref="SettingsPage">
            <Card>
                <CardMedia overlay={< CardTitle title = {<Av pathToPhoto={this.state.pathToPhoto} />} subtitle = {`${this.state.name} ${this.state.surname}'s home`} />}>
                    <img className="Card-image" src={this.state.pathToCard}/>
                </CardMedia>
                <CardText>
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec mattis pretium massa. Aliquam erat volutpat. Nulla facilisi. Donec vulputate interdum sollicitudin. Nunc lacinia auctor quam sed pellentesque. Aliquam dui mauris, mattis quis lacus id, pellentesque lobortis odio.
                </CardText>
                <CardActions>
                    <Tabs onChange={this.handleChange} value={this.state.slideIndex}>
                        <Tab label="Upload avatar" value={0}/>
                        <Tab label="Upload card" value={1}/>
                        <Tab label="Update You info" value={2}/>
                        <Tab label="Add album" value={3}/>
                    </Tabs>
                    <SwipeableViews index={this.state.slideIndex} onChangeIndex={this.handleChange}>
                        <div className="UploadAvatar">
                            <ImageUpload url="api/Account/UploadUserAvatar" name="avatar" changeState={this.changingPathToAvatar}/>
                        </div>
                        <div style={styles.slide} className="UploadCard">
                            <ImageUpload url="api/Account/UploadUserCard" name="card" changeState={this.changingPathToCard}/>
                        </div>
                        <div style={styles.slide}>
                            <UpdateUserInfo changingUserInfo={this.changingUserInfo} email={this.state.email} name={this.state.name} surname={this.state.surname} nickname={this.state.nickname} address={this.state.address}/>
                        </div>
                        <div style={styles.slide}>
                            <AlbumsUpdate email={this.state.email}/>
                        </div>
                    </SwipeableViews>
            </CardActions>
        </Card>
        </div>
      );
    }
});
